import { Directive, Input, ViewContainerRef, TemplateRef, OnInit } from '@angular/core';
import { Permissions } from '@common/models';
import { AuthenticationService } from '@common/services';

@Directive({
  selector: '[lxftHasPermission]'
})
export class HasPermissionDirective implements OnInit {
  private requiredPermissions: Permissions[] = [];

  @Input()
  set lxftHasPermission(permissions: string | string[]) {
    const p = permissions instanceof Array ? permissions : [permissions];
    this.requiredPermissions = p.map(permission => Permissions[permission]);
    this.applyPermission();
  }

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.applyPermission();
  }

  private applyPermission(): void {
    this.authenticationService.hasPermissions(this.requiredPermissions).subscribe(result => {
      if (result) {
        this.viewContainer.createEmbeddedView(this.templateRef);
      } else {
        this.viewContainer.clear();
      }
    });
  }
}
