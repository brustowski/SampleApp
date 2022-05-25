import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionSectionComponent } from './accordion-section.component';
import {By} from "@angular/platform-browser";
import {DebugElement} from "@angular/core";

describe('AccordionSectionComponent', () => {
  let component: AccordionSectionComponent;
  let fixture: ComponentFixture<AccordionSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccordionSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccordionSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should toggle section opening', () => {
    component.opened = false;

    component.toggleSection();

    expect(component.opened).toEqual(true);
  });

  it('should show title text in header', () => {
    component.title = 'test title';
    const element: DebugElement = fixture.debugElement;
    const header = element.query(By.css('.container-header'));
    const div: HTMLElement = header.nativeElement;
    fixture.detectChanges();
    expect(div.textContent).toEqual('test title');
  });

  it('should open section on clicking on header', () => {
    component.opened = false;
    const element: DebugElement = fixture.debugElement;
    const header = element.query(By.css('.container-header'));
    const div: HTMLElement = header.nativeElement;
    div.click();
    expect(component.opened).toEqual(true);
  });

  it('should close section on clicking on header', () => {
    component.opened = true;
    const element: DebugElement = fixture.debugElement;
    const header = element.query(By.css('.container-header'));
    const div: HTMLElement = header.nativeElement;
    div.click();
    expect(component.opened).toEqual(false);
  });
});
