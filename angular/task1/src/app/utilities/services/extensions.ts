import { Pager } from '../models/pager';
declare global {
  interface ArrayBuffer {
    asString(this: ArrayBuffer): string;
  }

  interface Date {
    toLocalizedDate(this: Date): string;
  }

  interface Array<T> {
    paginate(this: Array<T>, pager: Pager): T[];
  }
}
ArrayBuffer.prototype.asString = function (): string {
  return String.fromCharCode.apply(null, new Uint8Array(this) as any);
};

Date.prototype.toLocalizedDate = function (): string {
  return this.toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

Array.prototype.paginate = function <T>(pager: Pager): T[] {
  return this.slice(
    pager.pageIndex * pager.pageSize,
    (pager.pageIndex + 1) * pager.pageSize
  );
};
export class Extensions {}
