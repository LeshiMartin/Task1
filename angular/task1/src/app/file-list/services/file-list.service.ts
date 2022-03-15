import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, ReplaySubject, Subject } from 'rxjs';
import { FileModel, IFileModel } from '../models/file-model';
import { HubService } from '@utilities/services/hub.service';
import { HttpClient } from '@angular/common/http';
import { map, switchMap, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { DialogService } from '../../utilities/services/dialog.service';
import { UploadComponent } from '../../utilities/components/upload/upload.component';
import { ToasterService } from '../../utilities/services/toaster.service';
import { PageEvent } from '@angular/material/paginator';
import { Pager } from '@utilities/models/pager';
import { PagedData } from '@utilities/models/pagedData';

@Injectable({
  providedIn: 'root',
})
export class FileListService {
  constructor(
    private hub: HubService,
    private dialogService: DialogService,
    private httpClient: HttpClient
  ) {
    this.hub.fileIsNotValid$.subscribe(this.onFileIsInValidated);
    this.hub.fileIsProcessed$.subscribe(this.onFileIsProcessed);
    this.hub.fileIsProcessing$.subscribe(this.onFileIsInProcess);
    this.onFileUploadedSubscription();
    this.onFetchedFilesSubscription();
    this.onFileIsInProcessSubscription();
    this.onFileInvalidatedSubscription();
    this.onFileProcessedSubscription();
    this.onPagerChangeSubscription();
    this.onPageData
      .pipe(
        map((x: { data: FileModel[]; pager: Pager }) => ({
          total: x.data.length,
          data: x.data.paginate(x.pager),
        }))
      )
      .subscribe(this.pagedData$);
    this.onDataForPaging
      .pipe(
        switchMap((data) => {
          return this.currentPager.pipe(map((pager) => ({ data, pager })));
        })
      )
      .subscribe(this.onPageData);
    this.onFilesChange.subscribe((x) => {
      this.onDataForPaging.next(x);
    });
  }
  private _files: FileModel[] = [];
  onFilesChange = new Subject<IFileModel[]>();

  private onIsFetchingChange = new BehaviorSubject<boolean>(false);
  isFetching$ = this.onIsFetchingChange.asObservable();

  private onDataForPaging = new Subject<any>();
  private onPageData = new Subject<any>();
  private onPagerChange = new Subject<any>();
  private currentPager = new BehaviorSubject<Pager>({
    pageIndex: 0,
    pageSize: 10,
  });
  pagedData$ = new BehaviorSubject<PagedData<FileModel>>({
    total: 0,
    data: [],
  });
  private onFetchedFiles = new Subject<any>();
  private onFileIsInProcess = new Subject<any>();
  private onFileIsInValidated = new Subject<any>();
  private onFileIsProcessed = new Subject<any>();
  private onFileIsUploaded = new Subject<any>();

  private onPagerChangeSubscription() {
    this.onPagerChange
      .pipe(
        map((pager: Pager) => {
          return {
            total: this._files.length,
            data: this._files.paginate(pager),
          };
        })
      )
      .subscribe(this.pagedData$);
  }

  private onFileUploadedSubscription() {
    this.onFileIsUploaded
      .pipe(
        map((file: IFileModel) => {
          this._files = [new FileModel(file), ...this._files];
          return [...this._files];
        })
      )
      .subscribe((x) => {
        this.onFilesChange.next(x);
      });
  }

  private onFileProcessedSubscription() {
    this.onFileIsProcessed
      .pipe(
        map((id) => {
          const item = this.getFileById(id);
          if (item) item.toProcessed();
          return [...this._files];
        })
      )
      .subscribe(this.onFilesChange);
  }

  private onFileInvalidatedSubscription() {
    this.onFileIsInValidated
      .pipe(
        map((id) => {
          const item = this.getFileById(id);
          if (item) item.toInValidated();
          return [...this._files];
        })
      )
      .subscribe(this.onFilesChange);
  }

  private getFileById(id: any) {
    return this._files.find((x) => x.id == id);
  }

  private onFileIsInProcessSubscription() {
    this.onFileIsInProcess
      .pipe(
        map((id) => {
          const item = this.getFileById(id);
          if (item) item.toInProcess();
          return [...this._files];
        })
      )
      .subscribe(this.onFilesChange);
  }
  private onFetchedFilesSubscription() {
    this.onFetchedFiles
      .pipe(
        tap(
          (items: IFileModel[]) =>
            (this._files = [...items.map((x) => new FileModel(x))])
        )
      )
      .subscribe(this.onFilesChange);
  }

  fetchFiles() {
    this.onIsFetchingChange.next(true);
    return this.httpClient
      .get<FileModel[]>(`${environment.apiServer}/files`)
      .subscribe((data) => {
        setTimeout(() => {
          this._files = [...data.map((x) => new FileModel(x))];
          this.onIsFetchingChange.next(false);
          this.onFilesChange.next(data);
        }, 200);
      });
  }

  uploadFile(file: File) {
    if (!file) throw new Error('The file is missing');
    const fd = new FormData();
    fd.append(file.name, file, file.name);
    const obs = this.httpClient.post(environment.apis().upload, fd, {
      responseType: 'arraybuffer',
      observe: 'events',
      reportProgress: true,
    });

    this.openFileUpload(obs, file.name).subscribe((d) =>
      this.onFileIsUploaded.next(d)
    );
  }

  pageChange(pager: Pager) {
    this.onPagerChange.next(pager);
  }

  private openFileUpload(
    obs: Observable<any>,
    fileName: string
  ): Observable<any> {
    return this.dialogService.openDialog(UploadComponent, {
      minWidth: '35%',
      disableClose: true,
      data: {
        obs,
        fileName,
      },
    });
  }
}
