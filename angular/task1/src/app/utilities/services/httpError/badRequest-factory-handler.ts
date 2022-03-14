import { HttpErrorResponse } from '@angular/common/http';
export abstract class BadRequestFactoryHandler {
  abstract Handle(error: HttpErrorResponse): string;
}
export class BadRequestFactoryProvider {
  getHandler(error: HttpErrorResponse): BadRequestFactoryHandler {
    switch (true) {
      case Array.isArray(error.error):
        return new ErrorArrayHandler();
      case error.error instanceof ArrayBuffer:
        return new ArrayBufferErrorHandler();
      case !!error.error.errors:
        return new ErrorObjectHandler();
      default:
        return new MessageHandler();
    }
  }
}
export class ArrayBufferErrorHandler implements BadRequestFactoryHandler {
  Handle(error: HttpErrorResponse): string {
    const msg = String.fromCharCode.apply(
      null,
      new Uint8Array(error.error) as any
    );
    return msg;
  }
}
export class ErrorArrayHandler implements BadRequestFactoryHandler {
  Handle(error: HttpErrorResponse): string {
    let errorMessage = '';
    for (const err of error.error) {
      errorMessage = `${errorMessage} ${err} /n`;
    }
    return errorMessage;
  }
}
export class ErrorObjectHandler implements BadRequestFactoryHandler {
  Handle(error: HttpErrorResponse): string {
    let errorMessage = '';
    for (const key in error.error.errors) {
      errorMessage = `${errorMessage}${key} : ${error.error.errors[key]} \n`;
    }
    return errorMessage;
  }
}
export class MessageHandler implements BadRequestFactoryHandler {
  Handle(error: HttpErrorResponse): string {
    return `Error:${error.error}`;
  }
}
