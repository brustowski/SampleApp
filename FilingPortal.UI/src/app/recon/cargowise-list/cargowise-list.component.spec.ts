import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargoWiseListComponent } from './cargowise-list.component';

describe('ReconListComponent', () => {
  let component: CargoWiseListComponent;
  let fixture: ComponentFixture<CargoWiseListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargoWiseListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargoWiseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
