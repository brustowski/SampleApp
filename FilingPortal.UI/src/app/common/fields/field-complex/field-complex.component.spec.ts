import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldComplexComponent } from './field-complex.component';

describe('FieldComplexComponent', () => {
  let component: FieldComplexComponent;
  let fixture: ComponentFixture<FieldComplexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldComplexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldComplexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
