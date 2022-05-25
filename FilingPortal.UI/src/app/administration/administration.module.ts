import { NgModule } from '@angular/core';
import { CommonModule as CommonAngularModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AdminRoutingModule } from './admin-routing.module';
import { CommonModule } from '@common/common.module';
import { NgxDatatableModule } from '@custom/ngx-datatable';
import { GridPageModule } from '@common/grid';
import { FiltersModule } from '@common/filters';

import { RulesConfigurationService } from './services';

import { RulesConfigurationPageComponent } from './rules-configuration-page';
import { RulesConfigurationListComponent } from './rules-configuration-list';
import { ClientListComponent } from './client-list';
import { AutoCreateListComponent } from './autocreate-list';

@NgModule({
  imports: [
    CommonAngularModule,
    RouterModule,
    AdminRoutingModule,
    CommonModule,
    GridPageModule,
    NgxDatatableModule,
    FiltersModule
  ],
  providers: [RulesConfigurationService],
  declarations: [
    RulesConfigurationPageComponent,
    RulesConfigurationListComponent,
    ClientListComponent,
    AutoCreateListComponent
  ]
})
export class AdministrationModule {}
