import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeNodeCollapsibleComponent } from './filing-parameters-tree-node-collapsible.component';

describe('FilingParametersTreeNodeCollapsibleComponent', () => {
  let component: FilingParametersTreeNodeCollapsibleComponent;
  let fixture: ComponentFixture<FilingParametersTreeNodeCollapsibleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeNodeCollapsibleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeNodeCollapsibleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
