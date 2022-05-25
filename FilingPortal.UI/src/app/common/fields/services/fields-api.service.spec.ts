import { TestBed, inject } from '@angular/core/testing';

import { FieldsApiService } from './fields-api.service';
import {HttpService} from "../../services/http.service";
import {Observable} from "rxjs";
import {Response} from "@angular/http";

class MockHttpService{
  get(url: string, options?: any): Observable<Response> {
    return new Observable<Response>();
  };
}
describe('FieldsApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FieldsApiService,
        {provide: HttpService, useClass: MockHttpService}]
    });
  });

  it('should be created', inject([FieldsApiService], (service: FieldsApiService) => {
    expect(service).toBeTruthy();
  }));
});
