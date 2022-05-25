import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilingParametersTreeComponent } from './filing-parameters-tree.component';

describe('FilingParametersTreeComponent', () => {
  let component: FilingParametersTreeComponent;
  let fixture: ComponentFixture<FilingParametersTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FilingParametersTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilingParametersTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
