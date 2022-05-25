import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldLookupComponent } from './field-lookup.component';

describe('FieldLookupComponent', () => {
  let component: FieldLookupComponent;
  let fixture: ComponentFixture<FieldLookupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldLookupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldLookupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
