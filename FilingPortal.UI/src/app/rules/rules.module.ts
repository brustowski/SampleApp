import { NgModule } from '@angular/core';
import { CommonModule as CommonAngularModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { RulesRoutingModule } from './rules-routing.module';
import { RulesPageComponent } from './rules-page/rules-page.component';
import { RuleListComponent } from './rule-list/rule-list.component';
import { CommonModule } from '../common';
import { NgxDatatableModule } from '@custom/ngx-datatable';
import { GridPageModule } from '../common/grid';
import { FiltersModule } from '../common/filters';

import { RulesApiService } from './services';
import { RulesService } from './services';
import { RulesConfigurationService } from './services';

import { RulesComponent } from './rules/rules.component';
import { FileUploadModule } from 'ng2-file-upload';


@NgModule({
  imports: [
    CommonAngularModule,
    RouterModule,
    RulesRoutingModule,
    CommonModule,
    NgxDatatableModule,
    GridPageModule,
    FiltersModule,
    FileUploadModule
  ],
  declarations: [
    RulesPageComponent,
    RuleListComponent,
    RulesComponent,
  ],
  providers: [
    RulesApiService,
    RulesService,
    RulesConfigurationService
  ]
})
export class RulesModule { }
