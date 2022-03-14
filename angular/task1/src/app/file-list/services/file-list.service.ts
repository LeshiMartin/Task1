import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, ReplaySubject, Subject } from 'rxjs';
import { FileModel, IFileModel } from '../models/file-model';
import { HubService } from '@utilities/services/hub.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map, tap, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { DialogService } from '../../utilities/services/dialog.service';
import { UploadComponent } from '../../utilities/components/upload/upload.component';
import { ToasterService } from '../../utilities/services/toaster.service';

@Injectable({
  providedIn: 'root',
})
export class FileListService {
  onFilesChange = new ReplaySubject<IFileModel[]>(1);
  files$ = this.onFilesChange.asObservable();
  private onIsFetchingChange = new BehaviorSubject<boolean>(false);
  isFetching$ = this.onIsFetchingChange.asObservable();
  constructor(
    private hub: HubService,
    private dialogService: DialogService,
    toaster: ToasterService,
    private httpClient: HttpClient
  ) {
    this.files$ = this.onFilesChange.asObservable();
    this.hub.fileIsNotValid$.subscribe(this.onFileIsInValidated);
    this.hub.fileIsProcessed$.subscribe(this.onFileIsProcessed);
    this.hub.fileIsProcessing$.subscribe(this.onFileIsInProcess);
    this.onFileUploadedSubscription();
    this.onFetchedFilesSubscription();
    this.onFileIsInProcessSubscription();
    this.onFileInvalidatedSubscription();
    this.onFileProcessedSubscription();
    this.onFetchFiles
      .pipe(tap(() => this.onIsFetchingChange.next(true)))
      .subscribe(() => this._fetchFiles().subscribe(this.onFetchedFiles));
  }

  private _files: FileModel[] = [];

  private onFetchFiles = new Subject<any>();
  private onFetchedFiles = new Subject<any>();

  private onFileIsInProcess = new Subject<any>();
  private onFileIsInValidated = new Subject<any>();
  private onFileIsProcessed = new Subject<any>();
  private onFileIsUploaded = new Subject<any>();

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
          const item = this._files.find((x) => x.id == id);
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
          const item = this._files.find((x) => x.id == id);
          if (item) item.toInValidated();
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

  private onFileIsInProcessSubscription() {
    this.onFileIsInProcess
      .pipe(
        map((id) => {
          const item = this._files.find((x) => x.id == id);
          if (item) item.toInProcess();
          return [...this._files];
        })
      )
      .subscribe(this.onFilesChange);
  }

  fetchFiles() {
    this.onIsFetchingChange.next(true);
    return this.httpClient
      .get<FileModel[]>(`${environment.apiServer}/files`)
      .subscribe((data) => {
        setTimeout(() => {
          this._files = [...data];
          this.onIsFetchingChange.next(false);
          this.onFilesChange.next(data);
        }, 1000);
      });
  }

  private _fetchFiles(): Observable<FileModel[]> {
    return this.httpClient
      .get<FileModel[]>(environment.apis().getFiles)
      .pipe(tap((x) => this.onIsFetchingChange.next(false)));
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
