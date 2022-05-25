import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NavigationTabConfig } from '@common/navigation-tabs';
import { Observable } from 'rxjs/Observable';
import { RulesConfigurationService } from '../services';

@Component({
  selector: 'lxft-rules-configuration-page',
  templateUrl: './rules-configuration-page.component.html',
})
export class RulesConfigurationPageComponent implements OnInit {

  navConfig: NavigationTabConfig;
  pageTitle$: Observable<string>;

  constructor(private route: ActivatedRoute, private configurationService: RulesConfigurationService) { }

  ngOnInit() {
    this.pageTitle$ = this.route.data.map((data: { title: string }) => data.title);
    this.navConfig = this.configurationService.getTabs();
  }

}
