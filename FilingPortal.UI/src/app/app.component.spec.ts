import { TestBed, async } from '@angular/core/testing';
import { Location, CommonModule as CommonModuleAngular } from '@angular/common';
import { AppComponent } from './app.component';
import { CommonModule } from './common';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { InboundBridgeModule } from './inbound-bridge';
import { inject } from '@angular/core/testing';
import { log } from 'util';
import { ClientsModule } from './clients';
import { ClientListComponent } from './clients/client-list';
import { InboundBridgeRailListComponent } from '@inbound/rail-list';

describe('AppComponent', () => {

  const routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent
      ],
      imports: [
        CommonModuleAngular,
        RouterTestingModule.withRoutes([
          {
            path: '/',
            component: AppComponent
          },
          {
            path: '/imports',
            component: InboundBridgeRailListComponent
          },
          {
            path: 'client-management',
            component: ClientListComponent
          }
        ]),
        CommonModule,
        InboundBridgeModule,
        ClientsModule
      ],
      providers: [{ provide: Router, useValue: routerSpy }]
    }).compileComponents();
  }));
  it('should create the app', () => {
    async(inject([Router, Location], (router: Router, location: Location) => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.debugElement.componentInstance;
      fixture.detectChanges();
      expect(app).toBeTruthy();
    }));
  });
  it(`should have as title 'Charter Smart Filing Portal'`, () => {
    async(inject([Router, Location], (router: Router, location: Location) => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.debugElement.componentInstance;
      fixture.detectChanges();
      const compiled = fixture.debugElement.nativeElement;
      expect(compiled.querySelector('title').textContent).toBe('Charter Smart Filing Portal');
    }));
  });
  it('should tell ROUTER to navigate when Imports link clicked', () => {
    async(inject([Router, Location], (router: Router, location: Location) => {
      const fixture = TestBed.createComponent(AppComponent);
      fixture.detectChanges();

      fixture.debugElement.query(By.css('a')).nativeElement.click();
      log(fixture.debugElement.query(By.css('a')).nativeElement);
      fixture.whenStable().then(() => {
        expect(location.path()).toEqual('/imports');
        console.log('after expect');
      });
    }));
  });
});
