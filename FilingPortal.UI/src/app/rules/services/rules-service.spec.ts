import { TestBed, inject } from '@angular/core/testing';

import { RulesService } from './rules.service';
import { forEach } from '@angular/router/src/utils/collection';

describe('RulesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RulesService]
    });
  });

  it('should be created', inject([RulesService], (service: RulesService) => {
    expect(service).toBeTruthy();
  }));

  it('should not change original rule', inject([RulesService], (service: RulesService) => {
    var originalRule = { Id: 10, options: {}, ViewMode: true };
    service.depersonalizeRule(originalRule);

    expect(originalRule.Id).toBe(10);
    expect(originalRule.options).toBeTruthy();
    expect(originalRule.ViewMode).toBe(true);
  }));

  it('should create depersonalized copy of original object', inject([RulesService], (service: RulesService) => {
    var originalRule = { Id: 10, options: {}, ViewMode: true, allOtherData: "test" };
    var newRule = service.depersonalizeRule(originalRule);

    expect(newRule.Id).toBe(0);
    expect(newRule.options).toBeUndefined();
    expect(newRule.ViewMode).toBeUndefined();
    Object.keys(originalRule)
      .filter(x => x != "Id" && x != "options" && x != "ViewMode").forEach(x => {
        expect(originalRule[x]).toBe(newRule[x]);
      })
  }));
});
