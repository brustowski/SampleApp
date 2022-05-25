import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpandableGridDetailedRowComponent } from './expandable-grid-detailed-row.component';

describe('ExpandableGridDetailedRowComponent', () => {
  let component: ExpandableGridDetailedRowComponent;
  let fixture: ComponentFixture<ExpandableGridDetailedRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpandableGridDetailedRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpandableGridDetailedRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
