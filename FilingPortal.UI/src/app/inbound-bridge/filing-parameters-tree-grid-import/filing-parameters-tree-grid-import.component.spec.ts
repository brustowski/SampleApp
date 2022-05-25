import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeGridImportComponent } from './filing-parameters-tree-grid-import.component';

describe('FilingParametersTreeGridImportComponent', () => {
  let component: FilingParametersTreeGridImportComponent;
  let fixture: ComponentFixture<FilingParametersTreeGridImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeGridImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeGridImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
