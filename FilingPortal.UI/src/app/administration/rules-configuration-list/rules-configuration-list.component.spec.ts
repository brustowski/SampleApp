import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RulesConfigurationListComponent } from './rules-configuration-list.component';

describe('RulesConfigurationListComponent', () => {
  let component: RulesConfigurationListComponent;
  let fixture: ComponentFixture<RulesConfigurationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RulesConfigurationListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RulesConfigurationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
