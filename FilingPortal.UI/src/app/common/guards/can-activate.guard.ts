import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../services/authenticationService';
import { Permissions } from '@common/models';

@Injectable()
export class CanActivateGuard implements CanActivate, CanActivateChild {

  constructor(private authenticationService: AuthenticationService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.CanActivateRoute(route, state);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.CanActivateRoute(childRoute, state);
  }

  private CanActivateRoute(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authenticationService.getUser().map(user => {
      // check is user registered
      if (!this.authenticationService.isUserRegistered(user)) {
        this.router.navigate(['/send-request']);
        return false;
      }

      // check permissions

      const requiredPermissions: Permissions[] = route.data.permissions;
      if (requiredPermissions && requiredPermissions.length) {
        const hasPermissions = requiredPermissions.some(x => user.Permissions.indexOf(x) !== -1);
        return hasPermissions;
      }

      return true;
    });
  }
}
