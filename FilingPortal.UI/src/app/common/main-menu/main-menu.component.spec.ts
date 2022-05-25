import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { MainMenuComponent } from './main-menu.component';

describe('MainMenuComponent', () => {
  let component: MainMenuComponent;
  let fixture: ComponentFixture<MainMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
          RouterTestingModule,
      ],
      declarations: [ MainMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have 5 links', () => {
    const menuElement: HTMLElement = fixture.nativeElement;
    const links = menuElement.querySelectorAll('li a');
    expect(links.length).toEqual(5);
  });

  it('should have 4 menu links to pages', () => {
    const menuElement: HTMLElement = fixture.nativeElement;
    const links = menuElement.querySelectorAll('li a:not(.menu-control)');
    expect(links.length).toEqual(4);
  });

  it('should have imports link', () => {
    const menuElement: HTMLElement = fixture.nativeElement;
    const inboundBridgeLink = menuElement.querySelector('li a.rail');
    expect(inboundBridgeLink.getAttribute('href')).toEqual('/imports');
  });
});
