import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldAddressComponent } from './field-address.component';

describe('FieldAddressComponent', () => {
  let component: FieldAddressComponent;
  let fixture: ComponentFixture<FieldAddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldAddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
