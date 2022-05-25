import { FormatDecimalDirective } from './format-decimal.directive';
import {DebugElement, Component} from '@angular/core';
import {TestBed} from '@angular/core/testing';
import {ComponentFixture} from '@angular/core/testing';
import {By} from '@angular/platform-browser';
import {NgControl} from '@angular/forms';

@Component({
  template: `<input type="text" lxftFormatNumber="">`
})
class TestInputNumberComponent {
}

describe('FieldNumberDirective', () => {
  let ngControl: NgControl;
  let component: TestInputNumberComponent;
  let fixture: ComponentFixture<TestInputNumberComponent>;
  let inputEl: DebugElement;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TestInputNumberComponent, FormatDecimalDirective],
      providers: [NgControl]
    });
    fixture = TestBed.createComponent(TestInputNumberComponent);
    component = fixture.componentInstance;
    inputEl = fixture.debugElement.query(By.css('input'));
  });

  it('should create an instance', () => {
    const directive = new FormatDecimalDirective(inputEl, ngControl);
    expect(directive).toBeTruthy();
  });
});
