import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CanadaImpTruckListComponent } from './canada-imp-truck-list.component';

describe('InbondListComponent', () => {
  let component: CanadaImpTruckListComponent;
  let fixture: ComponentFixture<CanadaImpTruckListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CanadaImpTruckListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CanadaImpTruckListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
