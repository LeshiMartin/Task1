import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ToasterService {
  constructor(private snack: MatSnackBar) {}

  error(msg: string) {
    this.snack.open(msg, 'Error', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      direction: 'ltr',
      announcementMessage: msg,
      duration: 5000,
    });
  }
}
