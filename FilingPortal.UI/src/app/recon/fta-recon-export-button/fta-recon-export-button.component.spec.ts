import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FtaReconExportButtonComponent } from './fta-recon-export-button.component';

describe('CargowiseListExportComponent', () => {
  let component: FtaReconExportButtonComponent;
  let fixture: ComponentFixture<FtaReconExportButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FtaReconExportButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FtaReconExportButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
