import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UploadComponent } from './components/upload/upload.component';
import { LoadingTextComponent } from './components/loading-text/loading-text.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
@NgModule({
  declarations: [UploadComponent, LoadingTextComponent],
  imports: [CommonModule, MatDialogModule,MatSnackBarModule, MatIconModule, MatProgressBarModule],
  exports: [LoadingTextComponent],
})
export class UtilitiesModule {}
