import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MarksRemarksComponent } from './marks-remarks.component';

describe('MarksRemarksComponent', () => {
  let component: MarksRemarksComponent;
  let fixture: ComponentFixture<MarksRemarksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MarksRemarksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MarksRemarksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
