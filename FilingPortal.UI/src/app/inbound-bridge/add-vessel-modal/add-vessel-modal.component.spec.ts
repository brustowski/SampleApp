import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddVesselModalComponent } from './add-vessel-modal.component';

describe('AddVesselModalComponent', () => {
  let component: AddVesselModalComponent;
  let fixture: ComponentFixture<AddVesselModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddVesselModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddVesselModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
