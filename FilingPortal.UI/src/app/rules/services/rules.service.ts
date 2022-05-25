import { Injectable } from '@angular/core';

@Injectable()
export class RulesService {

  depersonalizeRule(rule: any): any {
    const newRule = { ...rule };
    newRule.Id = 0;
    delete newRule.options;
    delete newRule.ViewMode;
    return newRule;
  }

}
