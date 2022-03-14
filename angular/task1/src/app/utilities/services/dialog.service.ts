import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  constructor(private dialog: MatDialog) {}

  openDialog<T>(
    type: ComponentType<any>,
    option: MatDialogConfig<any>
  ): Observable<T> {
    this.dialog.closeAll();
    return this.dialog.open<T>(type, option).afterClosed();
  }
}
