import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';
@Injectable()
export class ToastrInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .catch((err: HttpEvent<any>, caught: Observable<HttpEvent<any>>) => {
        if (err instanceof HttpErrorResponse) {
          this.toastr.error(err.error.Message);
        }
        return of(err);
      });
  }
}
