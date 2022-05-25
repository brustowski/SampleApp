import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GridSelectionToolbarComponent } from './grid-selection-toolbar.component';

describe('GridSelectionToolbarComponent', () => {
  let component: GridSelectionToolbarComponent;
  let fixture: ComponentFixture<GridSelectionToolbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GridSelectionToolbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GridSelectionToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
