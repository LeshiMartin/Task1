import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BadRequestErrorHandler, NotFoundErrorHandler } from './httperror-handler-factory';
import {
  DefaultHttpErrorHandler,
  ErrorEventHandler,
  HttpErrorHandlerFactory,
  InternalServerErrorHandler,
  NoServerErrorHandler,
} from './httperror-handler-factory';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorFactoryProvider {
  getHandler(error: HttpErrorResponse): HttpErrorHandlerFactory {
    switch (true) {
      case error.error instanceof ErrorEvent:
        return new ErrorEventHandler();
      case error.status === 500:
        return new InternalServerErrorHandler();
      case error.status === 0:
        return new NoServerErrorHandler();
      case error.status === 400:
        return new BadRequestErrorHandler();
      case error.status === 404:
        return new NotFoundErrorHandler();
      default:
        return new DefaultHttpErrorHandler();
    }
  }
}
