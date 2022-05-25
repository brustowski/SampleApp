import { Component, OnInit, Output, EventEmitter, NgZone } from '@angular/core';
import { AuthenticationService, MainMenuService, EventsService } from '@common/services';
import { Permissions, MenuItem } from '@common/models';

@Component({
  selector: 'app-main-menu',
  templateUrl: './main-menu.component.html'
})
export class MainMenuComponent implements OnInit {

  @Output()
  toggle = new EventEmitter<boolean>();

  opened: boolean = true;
  userPermissions: Permissions[];
  menuItems: MenuItem[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private mainMenuService: MainMenuService,
    private events: EventsService) {
  }

  ngOnInit() {
    this.authenticationService.getUser().subscribe(user => {
      this.userPermissions = user ? user.Permissions : [];
      let items = this.mainMenuService.getMenuItems();
      items = items.filter(x => !x.permissions || this.hasPermission(x.permissions));
      items
        .filter(x => x.children && x.children.length)
        .forEach(x => x.children = x.children.filter(c => !c.permissions || this.hasPermission(c.permissions)));
      this.menuItems = items;
    });
  }

  hasPermission(requiredPermissions: Permissions[]): boolean {
    return this.userPermissions && requiredPermissions.some(x => this.userPermissions.indexOf(x) !== -1);
  }

  toggleSidebar(): void {
    this.opened = !this.opened;
    this.toggle.emit(this.opened);
    setTimeout(() => {
        this.events.updateGridSize$.emit();
    }, 700);
  }
}
