import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeNodeFieldsComponent } from './filing-parameters-tree-node-fields.component';

describe('FilingParametersTreeNodeFieldsComponent', () => {
  let component: FilingParametersTreeNodeFieldsComponent;
  let fixture: ComponentFixture<FilingParametersTreeNodeFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeNodeFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeNodeFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
