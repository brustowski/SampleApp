import { Injectable, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { InboundType } from '@inbound/models';

@Injectable({
  providedIn: 'root'
})
export class InboundTypeWatcherService implements OnDestroy {
  public get inboundType() {
    return this._inboundType;
  }

  private _inboundType: InboundType;

  routerEventsSubscription: Subscription;
  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    this.routerEventsSubscription = this.router.events
      .filter(event => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map(route => {
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      })
      .filter(route => route.outlet === 'primary')
      .mergeMap(route => route.data)
      .subscribe(data => (this._inboundType = data['type']));
  }

  ngOnDestroy(): void {
    this.routerEventsSubscription.unsubscribe();
  }
}
