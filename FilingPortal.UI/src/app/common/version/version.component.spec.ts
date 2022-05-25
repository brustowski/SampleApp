import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VersionComponent } from './version.component';
import {VersionApiService} from "./version-api.service";
import {RouterTestingModule} from "@angular/router/testing";
import {Observable} from "rxjs";
import {VersionModel} from "./VersionModel";

class MockVersionApiService{
  getVersion(): Observable<any>{
   return Observable.of(
     {
       json: () => {
         return new Observable<any>();
       }
     });
  }
}

describe('VersionComponent', () => {
  let service: any;
  let component: VersionComponent;
  let fixture: ComponentFixture<VersionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VersionComponent],
      providers: [
        {provide: VersionApiService,  useClass: MockVersionApiService}
      ],
      imports: [ RouterTestingModule ]
    }).compileComponents();
    service = TestBed.get(VersionApiService);
    fixture = TestBed.createComponent(VersionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call app version service to get version', () => {
    spyOn(service, 'getVersion');
    service.getVersion.and.returnValue(Observable.of(
        {
          json: () => {
            return new Observable<any>();
          }
        }
    ));
    component.ngOnInit();
    expect(service.getVersion).toHaveBeenCalled();
  });

  /*
  it('should have version model after got version', () => {
    spyOn(service, 'getVersion');
    service.getVersion.and.returnValue(Observable.of(
        {
          json: () => {
            const v: VersionModel = {
              AppVersion: '1.0.0.0',
              ShortAppVersion: '1.0',
              AppBuildDate: '4/9/2018 10:18:31 AM UTC'
            };
            return v;
          }
        }
    ));
    component.ngOnInit();
    service.getVersion().subscribe(() => {}, ()=> {}, () => {
      expect(component.version.AppVersion).toEqual('1.0.0.0');
      expect(component.version.ShortAppVersion).toEqual('1.0');
      expect(component.version.AppBuildDate).toEqual('4/9/2018 10:18:31 AM UTC');
    });
  });
  */
});

