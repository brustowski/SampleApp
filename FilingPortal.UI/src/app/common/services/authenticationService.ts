import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { controllerPath } from '@app/utils';
import { AppUserViewModel, Permissions } from '@common/models';
import { HttpClient } from '@angular/common/http';

import 'rxjs/add/operator/shareReplay';

@Injectable()
export class AuthenticationService {
  private currentUser$: Observable<AppUserViewModel>;

  constructor(private http: HttpClient) {
    this.currentUser$ = http
      .get<AppUserViewModel>(`${controllerPath}auth/current-user`)
      .shareReplay(1);
  }

  getUser(): Observable<AppUserViewModel> {
    return this.currentUser$;
  }

  sendRequest(requestInfo: string): Observable<Object> {
    const serviceUrl = `${controllerPath}auth/sendrequest`;
    return this.http.post(serviceUrl, { RequestInfo: requestInfo });
  }

  isUserRegistered(user: AppUserViewModel): boolean {
    return user != null && user.Status === 'Active';
  }

  hasPermissions(permissions: Permissions[]): Observable<boolean> {
    return this.currentUser$.map(user => {
      return !permissions || permissions.some(
        permission => user.Permissions.indexOf(permission) !== -1
      );
    });
  }
}