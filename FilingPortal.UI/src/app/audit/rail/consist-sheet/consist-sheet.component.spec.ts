import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsistSheetComponent } from './consist-sheet.component';

describe('ConsistSheetComponent', () => {
  let component: ConsistSheetComponent;
  let fixture: ComponentFixture<ConsistSheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsistSheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsistSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
