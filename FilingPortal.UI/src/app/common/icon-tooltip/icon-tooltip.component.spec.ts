import { Component, Input } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { IconTooltipComponent } from './icon-tooltip.component';
import { IconType } from '.';

describe('IconTooltipComponent', () => {
  let component: IconTooltipComponent;
  let fixture: ComponentFixture<IconTooltipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [BrowserModule, NgbModule.forRoot()],
      declarations: [IconTooltipComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconTooltipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should contains "div" element with "success" class', () => {
    component.iconType = IconType.Success;
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css(`.${IconType.Success}`))).toBeTruthy();
  });

  it('should contains "div" element with "warning" class', () => {
    component.iconType = IconType.Warning;
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css(`.${IconType.Warning}`))).toBeTruthy();
  });

  it('should contains "div" element with "error" class', () => {
    component.iconType = IconType.Error;
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css(`.${IconType.Error}`))).toBeTruthy();
  });

  it('should contains "div" element with "info" class', () => {
    component.iconType = IconType.Info;
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css(`.${IconType.Info}`))).toBeTruthy();
  });

  it('should contains "span" element with "innerText" specified', () => {
    component.iconType = IconType.Info;
    component.innerText = 'some inner text';
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css('span'))).toBeTruthy();
  });

  it('should not contain "span" element when "innerText" is not specified', () => {
    component.iconType = IconType.Info;
    fixture.detectChanges();
    const debugElement = fixture.debugElement;
    expect(debugElement.query(By.css('span')) === null).toBeTruthy();
  });
});
