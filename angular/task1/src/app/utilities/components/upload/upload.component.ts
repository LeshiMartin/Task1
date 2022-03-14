import { Component, Inject, OnInit } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpEventType,
  HttpProgressEvent,
} from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpResponse } from '@microsoft/signalr';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss'],
})
export class UploadComponent implements OnInit {
  constructor(
    private ref: MatDialogRef<UploadComponent>,
    @Inject(MAT_DIALOG_DATA)
    private data: { obs: Observable<any>; fileName: string }
  ) {
    this.fileDownload = data.obs;
    this.fileName = data.fileName ?? 'doc';
  }
  uploadProgress: number = 0;
  bufferProgress: number = 0;
  uploadIsInProgres: boolean = false;
  fileDownload: Observable<any>;
  fileName: string;
  ngOnInit(): void {
    this.fileDownload
      .pipe(
        map((event: HttpEvent<any>) => {
          switch (event.type) {
            case HttpEventType.UploadProgress:
              this.calcProgress(event);
              return event;
            case HttpEventType.Response:
            default:
              return event;
          }
        })
      )
      .subscribe((data: HttpEvent<any> | any) => {
        if (!data['body']) return;
        setTimeout(() => {
          this.ref.close(
            JSON.parse(((data as any)['body'] as ArrayBuffer).asString())
          );
        }, 1000);
      });
  }

  private calcProgress(event: HttpProgressEvent) {
    let total = event.total ? event.total : 1;
    this.uploadProgress = Math.round((event.loaded * 100) / total);
    this.bufferProgress = 100 - this.uploadProgress;
  }
}
