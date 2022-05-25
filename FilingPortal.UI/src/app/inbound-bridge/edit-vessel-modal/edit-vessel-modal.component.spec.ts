import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditVesselModalComponent } from './edit-vessel-modal.component';

describe('EditVesselModalComponent', () => {
  let component: EditVesselModalComponent;
  let fixture: ComponentFixture<EditVesselModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditVesselModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditVesselModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
