import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValueReconExportButtonComponent } from './value-recon-export-button.component';

describe('ValueReconExportButtonComponent', () => {
  let component: ValueReconExportButtonComponent;
  let fixture: ComponentFixture<ValueReconExportButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValueReconExportButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValueReconExportButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
