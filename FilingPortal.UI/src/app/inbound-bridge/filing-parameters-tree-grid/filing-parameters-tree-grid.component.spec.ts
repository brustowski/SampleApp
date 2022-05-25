import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeGridComponent } from './filing-parameters-tree-grid.component';

describe('FilingParametersTreeGridComponent', () => {
  let component: FilingParametersTreeGridComponent;
  let fixture: ComponentFixture<FilingParametersTreeGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
