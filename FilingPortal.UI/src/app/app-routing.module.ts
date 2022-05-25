import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { CanDeactivateGuard } from './common/guards/can-deactivate.guard';

import { CanActivateGuard } from './common/guards/can-activate.guard';
import { SendRequestComponent } from './common/send-request';
import { DashboardComponent } from './dashboard/dashboard';

const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [CanActivateGuard] }, // default page route
  { path: 'send-request', component: SendRequestComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: Boolean(history.pushState) === false,
      preloadingStrategy: PreloadAllModules,
      enableTracing: false
    })
  ],
  exports: [RouterModule],
  providers: [CanDeactivateGuard]
})
export class AppRoutingModule {
  constructor() {}
}
