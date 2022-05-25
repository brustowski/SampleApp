import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule } from './common';
import { InboundBridgeModule } from './inbound-bridge';

import { AppComponent } from './app.component';
import { RulesModule } from './rules';
import { DashboardModule } from './dashboard';
import { AdministrationModule } from './administration';
import { CopyToClipboardComponent } from '@common/copy-to-clipboard/copy-to-clipboard.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, ToastNoAnimation, ToastNoAnimationModule } from 'ngx-toastr';
import { NgxDatatableModule } from '../../custom-node-modules/ngx-datatable';
import {NgxMaskModule} from 'ngx-mask';
import { AuditModule } from './audit/audit.module';
import { ReconModule } from './recon/recon.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    NgbModule,
    AppRoutingModule,
    CommonModule,
    InboundBridgeModule,
    RulesModule,
    DashboardModule,
    AdministrationModule,
    AuditModule,
    ReconModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxDatatableModule,
    NgxMaskModule.forRoot()
  ],
  exports: [],
  providers: [],
  entryComponents: [CopyToClipboardComponent],
  bootstrap: [AppComponent]
})
export class AppModule {}
