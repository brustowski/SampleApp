import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule as CommonAngularModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppRoutingModule } from '../app-routing.module';
import { FieldsModule } from './fields';
import { GridPageModule } from './grid';
import { FiltersModule } from './filters';
import { LocalStorageModule } from 'angular-2-local-storage';

import { VersionComponent } from './version';
import { MainMenuComponent } from './main-menu';
import { HeaderComponent } from './header';
import { LoaderComponent } from './loader';
import { ColoredLabelComponent } from './colored-label';
import { ConfirmationComponent } from './confirmation';
import { AccordionSectionComponent } from './accordion-section';
import { IconTooltipComponent } from './icon-tooltip';

import { VersionApiService } from './version/version-api.service';
import { HttpService } from './services/http.service';
import { EventsService } from './services/events.service';
import { LocalStorageService } from './services/local-storage.service';
import { ModalService } from './services/modal.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavigationTabsComponent } from './navigation-tabs';
import { SendRequestComponent } from './send-request';
import { AuthenticationService } from './services/authenticationService';
import { CanActivateGuard } from './guards/can-activate.guard';
import { FileUploadService, ConfigurationService, MainMenuService, LoaderService } from './services';
import { FileUploadModule } from 'ng2-file-upload';
import {
  FileUploadResultComponent,
  FileUploadDetailedResultComponent,
  ButtonFileUploaderComponent,
  ImportWithConfirmationButtonComponent,
  FileUploadValidationResultComponent
} from './file-uploader';
import { PageContentWrapperComponent } from './page-content-wrapper/page-content-wrapper.component';
import { HasPermissionDirective } from './auth';
import { FilterPipe } from './pipes/filter.pipe';
import { LineBreakPipe } from './pipes/line-break.pipe';
import { CopyToClipboardDirective } from './directives/copy-to-clipboard.directive';
import { CopyToClipboardComponent } from './copy-to-clipboard/copy-to-clipboard.component';
import { ClipboardModule } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoaderInterceptor, ToastrInterceptor, CacheInterceptor } from './interceptors';
import { LayoutService } from './services/layout.service';
import { StopClickPropagationDirective } from './directives/stop-click-propagation.directive';
import { FileUploadModalComponent } from './file-uploader';

@NgModule({
  imports: [
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    CommonAngularModule,
    FileUploadModule,
    FieldsModule,
    GridPageModule,
    FiltersModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    LocalStorageModule.withConfig({
      prefix: 'app',
      storageType: 'localStorage'
    }),
    ClipboardModule,
  ],
  declarations: [
    VersionComponent,
    MainMenuComponent,
    HeaderComponent,
    LoaderComponent,
    ColoredLabelComponent,
    ConfirmationComponent,
    AccordionSectionComponent,
    IconTooltipComponent,
    NavigationTabsComponent,
    SendRequestComponent,
    FileUploadResultComponent,
    PageContentWrapperComponent,
    HasPermissionDirective,
    FilterPipe,
    LineBreakPipe,
    CopyToClipboardDirective,
    CopyToClipboardComponent,
    StopClickPropagationDirective,
    FileUploadModalComponent,
    ButtonFileUploaderComponent,
    FileUploadDetailedResultComponent,
    FileUploadValidationResultComponent,
    ImportWithConfirmationButtonComponent,
  ],
  exports: [
    VersionComponent,
    MainMenuComponent,
    HeaderComponent,
    LoaderComponent,
    ColoredLabelComponent,
    AccordionSectionComponent,
    FiltersModule,
    FieldsModule,
    IconTooltipComponent,
    NavigationTabsComponent,
    PageContentWrapperComponent,
    HasPermissionDirective,
    FilterPipe,
    LineBreakPipe,
    CopyToClipboardDirective,
    CopyToClipboardComponent,
    StopClickPropagationDirective,
    ButtonFileUploaderComponent,
    ImportWithConfirmationButtonComponent,
  ],
  entryComponents: [
    ConfirmationComponent,
    FileUploadResultComponent,
    FileUploadDetailedResultComponent,
    FileUploadValidationResultComponent,
    FileUploadModalComponent
  ],
  providers: [
    VersionApiService,
    EventsService,
    LocalStorageService,
    ModalService,
    FileUploadService,
    LoaderService,
    {
      provide: HttpService,
      useFactory: (http: HttpClient, toastr: ToastrService, loaderService: LoaderService) => {
        return new HttpService(http, toastr, loaderService);
      },
      deps: [HttpClient, ToastrService, LoaderService]
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CacheInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ToastrInterceptor,
      multi: true
    },
    AuthenticationService,
    ConfigurationService,
    CanActivateGuard,
    MainMenuService,
    FilterPipe,
    LayoutService
  ]
})
export class CommonModule { }
