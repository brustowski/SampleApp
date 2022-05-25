import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAuditComponent } from './daily-audit.component';

describe('DailyAuditComponent', () => {
  let component: DailyAuditComponent;
  let fixture: ComponentFixture<DailyAuditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyAuditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
