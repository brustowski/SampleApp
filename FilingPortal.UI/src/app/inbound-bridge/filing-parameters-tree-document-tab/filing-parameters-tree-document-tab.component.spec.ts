import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeDocumentTabComponent } from './filing-parameters-tree-document-tab.component';

describe('FilingParametersTreeDocumentTabComponent', () => {
  let component: FilingParametersTreeDocumentTabComponent;
  let fixture: ComponentFixture<FilingParametersTreeDocumentTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeDocumentTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeDocumentTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
