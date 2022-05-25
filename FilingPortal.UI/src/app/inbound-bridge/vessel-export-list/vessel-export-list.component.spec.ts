import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VesselExportListComponent } from './vessel-export-list.component';

describe('TruckExportListComponent', () => {
  let component: VesselExportListComponent;
  let fixture: ComponentFixture<VesselExportListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VesselExportListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VesselExportListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
