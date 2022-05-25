import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TruckExportFilingConfirmationDialogComponent } from './truck-export-filing-confirmation-dialog.component';

describe('TruckExportFilingConfirmationDialogComponent', () => {
  let component: TruckExportFilingConfirmationDialogComponent;
  let fixture: ComponentFixture<TruckExportFilingConfirmationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TruckExportFilingConfirmationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TruckExportFilingConfirmationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
