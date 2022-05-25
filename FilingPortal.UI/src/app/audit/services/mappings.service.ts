import { Injectable, OnDestroy } from '@angular/core';
import { Observable, of, Subscribable, Subscription } from 'rxjs';
import { InboundType } from '@inbound/models';
import { locationPath } from '@app/utils';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class MappingsService implements OnDestroy {
  private inboundType: InboundType;
  private apiPath: Map<string, string> = new Map<string, string>();

  routerEventsSubscription: Subscription;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    this.apiPath.set(InboundType.Rail, `${locationPath}/audit/rail`);
    this.apiPath.set(InboundType.DailyAudit, `${locationPath}/audit/rail/daily-audit`)

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
      .subscribe(data => (this.inboundType = data['type']));
  }

  ngOnDestroy(): void {
    this.routerEventsSubscription.unsubscribe();
  }

  private getApiPathByType(type: InboundType): string {
    return this.apiPath.get(type);
  }

  getApiPath(): Observable<string> {
    return of(this.getApiPathByType(this.inboundType));
  }
}
