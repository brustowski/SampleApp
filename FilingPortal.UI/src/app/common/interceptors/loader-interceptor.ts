import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { LoaderService } from '@common/services';
@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  constructor(public loader: LoaderService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loader.showLoader();
    return next.handle(request)
        .finally(() => this.loader.hideLoader());
  }
}
