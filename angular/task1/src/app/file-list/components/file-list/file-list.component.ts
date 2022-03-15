import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IFileModel, FileModelValues } from '../../models/file-model';
import { FileListService } from '../../services/file-list.service';
import { Pager } from '../../../utilities/models/pager';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileListComponent implements OnInit {
  constructor(private fileListService: FileListService) {
    this.fileListService.fetchFiles();
    this.files$ = this.fileListService.pagedData$.pipe(map((x) => x.data));
    this.isFetching$ = this.fileListService.isFetching$;
    this.total$ = this.fileListService.pagedData$.pipe(map((x) => x.total));
  }

  total$: Observable<number>;
  fileModelValues = FileModelValues;
  isFetching$: Observable<boolean>;
  files$: Observable<IFileModel[]> | undefined;
  ngOnInit(): void {}
  trackBy(index: number, item: IFileModel): { id: number; status: number } {
    return { id: item.id, status: item.fileStatusValue };
  }
  onFileChange(inp: HTMLInputElement) {
    const file = inp.files?.item(0);
    this.fileListService.uploadFile(file as File);
    inp.value = '';
  }
  pageChange(ev: PageEvent) {
    this.fileListService.pageChange(ev);
  }
}
