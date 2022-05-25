import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RulesConfigurationPageComponent } from './rules-configuration-page.component';

describe('RulesConfigurationPageComponent', () => {
  let component: RulesConfigurationPageComponent;
  let fixture: ComponentFixture<RulesConfigurationPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RulesConfigurationPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RulesConfigurationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
