import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl } from '@angular/forms';
import { KeyCodes } from '@app/utils';

@Directive({
  selector: '[lxftFormatDecimal]'
})
export class FormatDecimalDirective {
  @Input() lxftFormatDecimal: any;
  private regexNotAllowedValues = new RegExp('[^0-9.+-]');
  private regexValidTempalte = new RegExp('^[-+]?([0-9]+\.)?[0-9]*$');

  constructor(private elem: ElementRef, private control: NgControl) {}

  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    return this.isValidKey(event);
  }

  private isValidKey(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    const selectionStart = this.elem.nativeElement.selectionStart;
    const selectionEnd = this.elem.nativeElement.selectionEnd;

    if (
      (charCode >= KeyCodes.Digit0 && charCode <= KeyCodes.Digit9) ||
      (charCode >= KeyCodes.Numpad0 && charCode <= KeyCodes.Numpad9) ||
      charCode === KeyCodes.Add ||
      charCode === KeyCodes.NumpadAdd ||
      charCode === KeyCodes.NumpadSubstruct ||
      charCode === KeyCodes.Substruct ||
      charCode === KeyCodes.Period
    ) {
      const prev: string = this.control.value ? this.control.value : '';
      const value = prev.slice(0, selectionStart) + event.key + prev.slice(selectionEnd);
      return this.regexValidTempalte.test(value);
    }

    return false;
  }

  @HostListener('input', ['$event'])
  onChange(event: Event) {
    if (this.elem.nativeElement === event.target) {
      const value: string = this.control.value;
      if (this.regexNotAllowedValues.test(value)) {
        const newValue = value.replace(this.regexNotAllowedValues, '');
        this.control.control.setValue(newValue);
      }
    }
  }

  @HostListener('blur', ['$event'])
  onBlur(event: Event) {
    if (this.elem.nativeElement === event.target) {
      const value: string = this.control.value;
      if (value.indexOf('.') === value.length - 1) {
        const newValue = value.slice(0, value.length - 1);
        this.control.control.setValue(newValue);
      }
    }
  }
}
