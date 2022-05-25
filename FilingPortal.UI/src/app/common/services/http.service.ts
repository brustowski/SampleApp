import { Injectable } from '@angular/core';
import { Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import { LoaderService } from './loader.service';
import { ToastrService } from 'ngx-toastr';
import { HttpClient, HttpParams } from '@angular/common/http';
import { throwError } from 'rxjs';

// This is wrapper service for http requests
@Injectable()
export class HttpService {
  constructor(
    private httpClient: HttpClient,
    private toastr: ToastrService,
    protected loaderService: LoaderService
  ) { }

  request(config: any, options?): Observable<any> {
    // TODO: save version and send to backend
    /*
        const version = this.session.getVersion();
        const isAddVersion = config.url.indexOf(locationPath) > -1;
        if (isAddVersion && version) {
            config.headers.set('X-Client-Version', version);
        }
        */
    const timestamp =
      config.url.indexOf('?') > -1 ? '&timestamp=' : '?timestamp=';
    config.url = config.url + timestamp + new Date().getTime();

    this.loaderService.showLoader();

    return this.httpClient.request(config, options)
      .catch(this.catchAuthError(this))
      .finally(() => { this.loaderService.hideLoader(); });
  }

  private catchAuthError(self: HttpService) {
    return (res: Response) => {
      // TODO: handling errors
      if (res.status === 410) {
        // window.location.reload();
      }

      if (res.status === 500) {
        let msg = 'Error occurred';
        msg += res.statusText ? `: ${res.statusText}` : '';
        this.toastr.error(msg);
      }

      if (res.status === 400) {
        const result = res.json();
        this.toastr.error(result.Message);
      }

      if (res.status === 401) {
        /*if (res.headers.get('x-unauthorized') === headerAuthFail) {
                    this.authService.clearSessionData();
                } else {
                    this.notify.alert(permissionsDeniedMsg);
                }*/
      }

      if (res.status === 403) {
        const result = res.json();
        this.toastr.error(result.Message);
      }

      return throwError(res.json());
    };
  }

  requestRaw(url: string, formData: FormData) {
    const notify = this.toastr;
    return new Observable((observer: Observer<any>) => {
      const xhr = new XMLHttpRequest();
      this.loaderService.showLoader();
      xhr.open('POST', url, true);
      xhr.onload = () => {
        const jsonResponse = JSON.parse(xhr.responseText);
        const messages: string[] = jsonResponse.Messages;
        this.loaderService.hideLoader();
        if (xhr.status === 200) {
          observer.next(jsonResponse);
          observer.complete();
        }
        if (xhr.status === 500) {
          let msg = 'Error occurred';
          msg += xhr.statusText ? `: ${xhr.statusText}` : '';
          notify.error(msg);
          observer.error(xhr.response);
        }

        if (xhr.status === 400) {
          messages.forEach(msg => notify.error(msg));
          observer.error(xhr.response);
        }
        if (xhr.status === 403) {
          messages.forEach(msg => notify.error(msg));
          observer.error(xhr.response);
        }
      };

      xhr.send(formData);
    });
  }
}