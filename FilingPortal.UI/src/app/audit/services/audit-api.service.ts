import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MappingsService } from './mappings.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuditApiService {

  constructor(private http: HttpClient, private mappings: MappingsService) { }

  verify() {
    return this.mappings
      .getApiPath()
      .switchMap(path =>
        this.http.post(`${path}/train-consist-sheet/verify`, null)
      );
  }

  clear() {
    return this.mappings
    .getApiPath()
    .switchMap(path =>
      this.http.delete(`${path}/train-consist-sheet/clear`)
    );
  }

  report(params: any): Observable<Object> {
    return this.mappings
      .getApiPath()
      .switchMap(path =>
        this.http.post(`${path}/report`, params)
      );
  }
}
