import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpandableGridFilingComponent } from './expandable-grid-filing.component';

describe('ExpandableGridFilingComponent', () => {
  let component: ExpandableGridFilingComponent;
  let fixture: ComponentFixture<ExpandableGridFilingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpandableGridFilingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpandableGridFilingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
