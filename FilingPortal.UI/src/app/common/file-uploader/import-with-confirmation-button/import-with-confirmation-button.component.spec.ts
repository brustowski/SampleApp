import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportWithConfirmationButtonComponent } from './import-with-confirmation-button.component';

describe('ImportWithConfirmationButtonComponent', () => {
  let component: ImportWithConfirmationButtonComponent;
  let fixture: ComponentFixture<ImportWithConfirmationButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportWithConfirmationButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportWithConfirmationButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
