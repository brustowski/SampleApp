import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { locationPath } from '../../utils';

import { AvailableActions } from '@common/models';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ConfigurationService {
  constructor(private http: HttpClient) {}

  private apiPath = `${locationPath}/settings/page-configuration`;

  getPageActions(pageName: string): Observable<AvailableActions> {
    return this.http
      .get<{Actions: AvailableActions}>(`${this.apiPath}?pageName=${pageName}`)
      .map(result => result.Actions); // todo: check for nul??
  }
}
