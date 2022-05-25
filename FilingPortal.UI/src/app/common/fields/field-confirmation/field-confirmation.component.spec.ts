import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldConfirmationComponent } from './field-confirmation.component';

describe('FieldConfirmationComponent', () => {
  let component: FieldConfirmationComponent;
  let fixture: ComponentFixture<FieldConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
