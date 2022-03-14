import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IFileModel, FileModelValues } from '../../models/file-model';
import { FileListService } from '../../services/file-list.service';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss'],
})
export class FileListComponent implements OnInit {
  constructor(private fileListService: FileListService) {
    this.fileListService.fetchFiles();
    this.files$ = this.fileListService.files$;
    this.isFetching$ = this.fileListService.isFetching$;
  }

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

  downloadSample() {
    window.open('/src/assets/samples/Sample.txt', '_blank');
  }
}
