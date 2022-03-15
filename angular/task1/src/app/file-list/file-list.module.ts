import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FileListComponent } from './components/file-list/file-list.component';
import { FileListRoutingModule } from './file-list-routing.module';
import { UtilitiesModule } from '../utilities/utilities.module';
@NgModule({
  declarations: [FileListComponent],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatSnackBarModule,
    MatPaginatorModule,
    UtilitiesModule,
  ],
  exports: [FileListRoutingModule],
})
export class FileListModule {}
