import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManifestComponent } from './manifest.component';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {By} from '@angular/platform-browser';
import {Observable} from 'rxjs/Observable';
import {InboundRecordsApiService} from '../services/inbound-records-api.service';

describe('ManifestComponent', () => {
  let component: ManifestComponent;
  let fixture: ComponentFixture<ManifestComponent>;

  const fakeApiService: any =  { getManifest: (manifestId: any) => new Observable<any>() };
  fakeApiService.getManifest = jasmine.createSpy('getManifest', fakeApiService.getManifest)
    .and.returnValue({ subscribe: () => 'some text' });

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManifestComponent ],
      providers: [NgbActiveModal,
        {provide: InboundRecordsApiService, useValue: fakeApiService}]
    });
    fixture = TestBed.createComponent(ManifestComponent);
    component = fixture.componentInstance;
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call api service on init', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    component.modalInfo = {manifestId: 1};
    component.ngOnInit();
    expect(apiService.getManifest).toHaveBeenCalled();
  });
});
