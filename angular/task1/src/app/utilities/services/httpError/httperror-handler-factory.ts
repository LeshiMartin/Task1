import { HttpErrorResponse } from '@angular/common/http';
import { BadRequestFactoryProvider } from './badRequest-factory-handler';

export abstract class HttpErrorHandlerFactory {
  abstract Handle(error: HttpErrorResponse): string;
}
export class ErrorEventHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    return `Error: ${error.error.message}`;
  }
}
export class InternalServerErrorHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    return 'Error happened on the server please try again later';
  }
}
export class NoServerErrorHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    return 'There is no server listening to the url';
  }
}

export class NotFoundErrorHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    return 'The requested path doesn`t exist or has been moved';
  }
}
export class BadRequestErrorHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    let provider = new BadRequestFactoryProvider();
    let handler = provider.getHandler(error);
    return handler.Handle(error);
  }
}
export class DefaultHttpErrorHandler implements HttpErrorHandlerFactory {
  Handle(error: HttpErrorResponse): string {
    return error.message;
  }
}
