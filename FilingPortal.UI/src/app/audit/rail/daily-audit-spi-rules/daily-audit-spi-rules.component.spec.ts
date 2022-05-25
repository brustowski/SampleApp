import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyAuditSpiRulesComponent } from './daily-audit-spi-rules.component';

describe('DailyAuditRulesComponent', () => {
  let component: DailyAuditSpiRulesComponent;
  let fixture: ComponentFixture<DailyAuditSpiRulesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailyAuditSpiRulesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailyAuditSpiRulesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
