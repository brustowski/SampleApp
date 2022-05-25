import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IsfAddInboundModalComponent } from './isf-add-inbound-modal.component';

describe('AddVesselModalComponent', () => {
  let component: IsfAddInboundModalComponent;
  let fixture: ComponentFixture<IsfAddInboundModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IsfAddInboundModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IsfAddInboundModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
