import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReconService } from './recon.service';
import { Observable } from 'rxjs/Observable';
import { AvailableActions } from '@common/models';
import { PageParams } from '@common/grid/models';

@Injectable({
  providedIn: 'root'
})
export class ReconApiService {

  constructor(private http: HttpClient, private service: ReconService) { }

  report(params: any): Observable<Object> {
    return this.service
      .getApiPath()
      .switchMap(path =>
        this.http.post(`${path}/report`, params)
      );
  }

  getAvailableActions(ids: number[]): Observable<AvailableActions> {
    return this.service
      .getApiPath()
      .switchMap(path => this.http.post<AvailableActions>(`${path}/available-actions`, ids));
  }

  process(recordIds: (string | number)[]): Observable<void | number> {
    return this.service
      .getApiPath()
      .switchMap(path => this.http.post<void | number>(`${path}/process`, recordIds));
  }

  exportabilityCheck(pageParams: PageParams): Observable<string> {
    return this.service
      .getApiPath()
      .switchMap(path => this.http.post<string>(`${path}/exportability-check`, pageParams));
  }

  resetReport(): Observable<void> {
    return this.service
      .getApiPath()
      .switchMap(path => this.http.delete<void>(`${path}/ace-report`));
  }

  exportAceReport(params: any): void {
    this.service
      .getApiPath()
      .subscribe(path => {
        const str = `${path}/ace-report?data=${btoa(JSON.stringify(params))}`;
        window.open(str, '_blank');
      });
  }
}
