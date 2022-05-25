import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { locationPath } from '@app/utils';
import { ValidationResultWithFieldsErrorsViewModel } from '@common/models';

@Injectable()
export class RulesApiService {

  constructor(private http: HttpClient) { }

  deleteRule(pathForApi: string, ruleId: any): Observable<any> {
    return this.http.post(`${locationPath}/${pathForApi}/delete/${ruleId}`, {});
  }

  getNewRule<TRule>(pathForApi: string): Observable<any> {
    return this.http
      .get(`${locationPath}/${pathForApi}/getNew`);
  }

  updateRule<TRule>(pathForApi: string, rule: TRule): Observable<ValidationResultWithFieldsErrorsViewModel> {
    return this.http.post<ValidationResultWithFieldsErrorsViewModel>(`${locationPath}/${pathForApi}/update`, rule);
  }

  createRule<TRule>(pathForApi: string, rule: TRule): Observable<ValidationResultWithFieldsErrorsViewModel> {
    return this.http.post<ValidationResultWithFieldsErrorsViewModel>(`${locationPath}/${pathForApi}/create`, rule);
  }
}
