import {Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-colored-label',
  templateUrl: './colored-label.component.html'
})
export class ColoredLabelComponent {
  @Input() title: string;
  @Input() css: string;
  @Input() value: string;

  constructor() { }
}
