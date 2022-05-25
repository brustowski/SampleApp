import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargowiseListExportComponent } from './cargowise-list-export.component';

describe('CargowiseListExportComponent', () => {
  let component: CargowiseListExportComponent;
  let fixture: ComponentFixture<CargowiseListExportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargowiseListExportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargowiseListExportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
