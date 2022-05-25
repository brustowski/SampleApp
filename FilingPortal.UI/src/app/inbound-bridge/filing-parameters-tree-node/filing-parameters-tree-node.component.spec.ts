import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeNodeComponent } from './filing-parameters-tree-node.component';

describe('FilingParametersTreeNodeComponent', () => {
  let component: FilingParametersTreeNodeComponent;
  let fixture: ComponentFixture<FilingParametersTreeNodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeNodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeNodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
