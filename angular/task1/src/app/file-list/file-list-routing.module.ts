import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FileListComponent } from './components/file-list/file-list.component';

const routes: Routes = [
  { path: '', component: FileListComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FileListRoutingModule {}
