import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have logo', () => {
    const menuElement: HTMLElement = fixture.nativeElement;
    const logoElement = menuElement.querySelector('.logo');
    expect(logoElement.textContent).toEqual('CHARTER BROKERAGE');
  });

  it('should have user name element', () => {
    const menuElement: HTMLElement = fixture.nativeElement;
    const userElement = menuElement.querySelector('.user-name');
    expect(userElement).toBeTruthy();
  });

});
