import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldSelectComponent } from './field-select.component';
import {NgSelectModule} from "@ng-select/ng-select";
import {FormsModule} from "@angular/forms";
import {FieldsApiService} from "../services/fields-api.service";
import {Observable} from "rxjs";

describe('FieldSelectComponent', () => {
  let component: FieldSelectComponent;
  let fixture: ComponentFixture<FieldSelectComponent>;

  const fakeFieldsApiService: any =  { getSelectFieldOptions: (searchSettings: any) => {return new Observable<any>();} };
  fakeFieldsApiService.getSelectFieldOptions = jasmine.createSpy('getSelectFieldOptions', fakeFieldsApiService.getSelectFieldOptions);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldSelectComponent ],
      imports: [NgSelectModule, FormsModule],
      providers: [
        {provide: FieldsApiService, useValue: fakeFieldsApiService}
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
