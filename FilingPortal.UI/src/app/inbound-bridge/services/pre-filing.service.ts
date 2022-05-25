import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PreFilingValidationResult } from '@common/models';
import { Observable } from 'rxjs';
import { InboundConfigurationService } from './inbound-configuration.service';

@Injectable({
  providedIn: 'root'
})
export class PreFilingSevice {

  constructor(
    private http: HttpClient,
    private mappingsService: InboundConfigurationService
  ) { }

  validateRecords(ids: number[]): Observable<PreFilingValidationResult[]> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<PreFilingValidationResult[]>(`${path}/filing/validate-records`, ids)
      );
  }
}
