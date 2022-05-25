import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAuditRulesComponent } from './daily-audit-rules.component';

describe('DailyAuditRulesComponent', () => {
  let component: DailyAuditRulesComponent;
  let fixture: ComponentFixture<DailyAuditRulesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyAuditRulesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyAuditRulesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
