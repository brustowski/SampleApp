import { TestBed } from '@angular/core/testing';

import { VersionApiService } from './version-api.service';
import {RouterTestingModule} from "@angular/router/testing";
import {HttpService} from "../services/http.service";
import {Observable} from "rxjs";
import {Response} from "@angular/http";

class MockHttpService{
  get(url: string, options?: any): Observable<Response> {
    return new Observable<Response>();
  };
}

describe('VersionApiService', () => {
  let service: VersionApiService;
  let httpService: any;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VersionApiService,
        {provide: HttpService, useClass: MockHttpService}
      ],
      imports: [ RouterTestingModule ]
    });
    service = TestBed.get(VersionApiService);
    httpService = TestBed.get(HttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should call http service when getting app version', () => {
    spyOn(httpService, 'get');
    httpService.get.and.returnValue(Observable.of(
        {
          json: () => {
            return new Observable<Response>();
          }
        }
        )
    );
    service.getVersion();
    expect(httpService.get).toHaveBeenCalled();
  });
});
