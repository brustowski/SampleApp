import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldFloatNumberComponent } from './field-float-number.component';

describe('FieldFloatNumberComponent', () => {
  let component: FieldFloatNumberComponent;
  let fixture: ComponentFixture<FieldFloatNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldFloatNumberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldFloatNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
