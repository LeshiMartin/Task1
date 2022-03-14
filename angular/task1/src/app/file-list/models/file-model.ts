export interface IFileModel {
  id: number;
  fileName: string;
  fileStatusValue: number;
  fileStatus: string;
  dateOfUpload: string;
}

export class FileModel implements IFileModel {
  constructor(init?: Partial<IFileModel>) {
    Object.assign(this, init);
  }
  id: number = 0;
  fileName: string = '';
  fileStatusValue: number = 0;
  fileStatus: string = '';
  dateOfUpload: string = '';

  toInProcess() {
    this.fileStatusValue = FileModelValues.InProcess;
    this.fileStatus = 'InProcess';
  }

  toInValidated() {
    this.fileStatusValue = FileModelValues.InValid;
    this.fileStatus = 'InValid';
  }

  toProcessed() {
    this.fileStatusValue = FileModelValues.Processed;
    this.fileStatus = 'Processed';
  }
}

export enum FileModelValues {
  NotProcessed = 10,
  InProcess = 20,
  Processed = 30,
  InValid = 40,
}
