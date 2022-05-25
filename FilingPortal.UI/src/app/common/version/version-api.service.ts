import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { locationPath } from '../../utils';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VersionApiService {

  constructor(
    private http: HttpClient
  ) { }

  getVersion(): Observable<any> {
    return this.http
      .get(`${locationPath}/appversion/get`);
  }
}
