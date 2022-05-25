import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnFieldSelectorComponent } from './column-field-selector.component';

describe('ColumnFieldSelectorComponent', () => {
  let component: ColumnFieldSelectorComponent;
  let fixture: ComponentFixture<ColumnFieldSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColumnFieldSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnFieldSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
