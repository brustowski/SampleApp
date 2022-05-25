import { Injectable, OnDestroy } from '@angular/core';
import { Observable, of, Subscription, Subject, BehaviorSubject } from 'rxjs';
import { InboundType } from '@inbound/models';
import { locationPath } from '@app/utils';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { AvailableActions } from '@common/models';

@Injectable({
  providedIn: 'root'
})
export class ReconService implements OnDestroy {
  private inboundType: InboundType;
  private apiPath: Map<string, string> = new Map<string, string>();

  private availableActions = new BehaviorSubject<AvailableActions>({});
  public availableActions$ = this.availableActions.asObservable();

  routerEventsSubscription: Subscription;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    this.apiPath.set(InboundType.Recon, `${locationPath}/recon/cargowise`);
    this.apiPath.set(InboundType.FtaRecon, `${locationPath}/recon/fta`);
    this.apiPath.set(InboundType.ValueRecon, `${locationPath}/recon/value`);

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

  setActions(actions: AvailableActions): void {
    this.availableActions.next(actions);
  }
}
