import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'lxft-accordion-section',
  templateUrl: './accordion-section.component.html'
})
export class AccordionSectionComponent implements OnInit {
  @Input() title: string = '';
  @Input() opened: boolean = false;
  @Input() openedIcon: string;
  @Input() minimizedIcon: string;
  @Output() onToggle: EventEmitter<void> = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  toggleSection() {
    this.opened = !this.opened;
    this.onToggle.emit();
  }

  getClass(): string {
    return this.opened && this.openedIcon
      ? this.openedIcon
      : !this.opened && this.minimizedIcon
      ? this.minimizedIcon
      : 'icon-dropdown';
  }
}
