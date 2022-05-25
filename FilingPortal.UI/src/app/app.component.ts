import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';

import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  public defaultPageTitle = 'Charter Smart Filing Portal';
  private notifySubscription: any;
  public notifications: any[];

  leftMenuIsVisible: boolean = true;
  headerIsVisible: boolean = true;
  isClosedSidebar: boolean = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
  ) {}

  ngOnInit() {
    this.onRouteChange();
  }
  ngOnDestroy() {
    this.notifySubscription.unsubscribe();
  }

  onRouteChange() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map(route => {
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      })
      .filter(route => route.outlet === 'primary')
      .subscribe((activatedRoute: ActivatedRoute) => {
        const data: any = activatedRoute.snapshot.data;

        const pageTitle = data.title ? data.title : this.defaultPageTitle;

        this.titleService.setTitle(pageTitle);

        const sendRequestRoute =
          activatedRoute.routeConfig.path === 'send-request';
        this.headerIsVisible = !sendRequestRoute;
        this.leftMenuIsVisible = !sendRequestRoute;
      });
  }
}
