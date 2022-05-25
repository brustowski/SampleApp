import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgControl } from '@angular/forms';
import { KeyCodes } from '@app/utils';

@Directive({
  selector: '[lxftFormatNumber]'
})
export class FormatNumberDirective {
  @Input() lxftFormatNumber: any;
  private regexNotAllowedValues = new RegExp('[^0-9+-]');
  private regexValidTempalte = new RegExp('^[-+]?[0-9]*$');

  constructor(private elem: ElementRef, private control: NgControl) {}

  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    return this.isValidKey(event);
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
      charCode === KeyCodes.Substruct
    ) {
      const prev: string = this.control.value ? this.control.value : '';
      const value = prev.slice(0, selectionStart) + event.key + prev.slice(selectionEnd);
      return this.regexValidTempalte.test(value);
    }

    return false;
  }
}
