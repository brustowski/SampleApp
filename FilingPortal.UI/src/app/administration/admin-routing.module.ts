import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CanDeactivateGuard } from '@common/guards/can-deactivate.guard';
import { CanActivateGuard } from '@common/guards/can-activate.guard';

import { Permissions } from '@common/models';

import { PageContentWrapperComponent } from '@common/page-content-wrapper';
import { RulesConfigurationPageComponent } from './rules-configuration-page';
import { RulesConfigurationListComponent } from './rules-configuration-list';
import { ClientListComponent } from './client-list';
import { AutoCreateListComponent } from './autocreate-list';

const routes: Routes = [
  {
    path: 'admin',
    component: PageContentWrapperComponent,
    data: { title: 'Administration' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      {
        path: '',
        redirectTo: 'rules-configuration',
        pathMatch: 'full'
      },
      {
        path: 'rules-configuration',
        component: RulesConfigurationPageComponent,
        data: { title: 'Rules Configuration' },
        children: [
          {
            path: '',
            redirectTo: 'rail-import',
            pathMatch: 'full'
          },
          {
            path: ':name',
            component: RulesConfigurationListComponent,
            data: { title: 'Rules Configuration', permissions: [Permissions.ViewConfiguration, Permissions.EditConfiguration] },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'client-management',
        component: ClientListComponent,
        data: {
          title: 'Client Management',
          permissions: [Permissions.ViewClients]
        }

      },
      {
        path: 'auto-create',
        component: AutoCreateListComponent,
        data: {
          title: 'Auto-create configuration',
          permissions: [Permissions.AdminAutoCreateConfiguration]
        }

      },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
