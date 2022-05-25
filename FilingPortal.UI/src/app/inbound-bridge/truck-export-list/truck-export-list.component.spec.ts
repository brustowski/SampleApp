import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TruckExportListComponent } from './truck-export-list.component';

describe('TruckExportListComponent', () => {
  let component: TruckExportListComponent;
  let fixture: ComponentFixture<TruckExportListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TruckExportListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TruckExportListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
