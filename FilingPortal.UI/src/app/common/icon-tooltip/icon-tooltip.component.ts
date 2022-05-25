import {
  Component, OnInit, Input, ViewChild, AfterViewChecked, NgZone
} from '@angular/core';
import { IconType } from './icon-tooltip.model';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'lxft-icon-tooltip',
  templateUrl: './icon-tooltip.component.html'
})
export class IconTooltipComponent implements OnInit, AfterViewChecked {

  @Input() innerText: string = null;
  @Input() messages: string[];
  @Input() iconType: IconType;
  @Input() counterOn: boolean = true;

  @ViewChild('p') public popover: NgbPopover;

  constructor(
    protected zone: NgZone) {
  }

  ngOnInit(): void {
  }

  ngAfterViewChecked() {
    this.applyStyle();
  }

  applyStyle() {
    if (this.popover && this.popover.isOpen()) {
      const id = (this.popover as any)._ngbPopoverWindowId;
      const elem = document.getElementById(id);
      elem.classList.add(`${this.iconType}-popover`);
    }
  }

  getClass(): string {
    return this.iconType;
  }

  mouseEnter() {
    this.zone.run(() => {
      this.popover.open();
    });
  }

  mouseLeave() {
    this.popover.close();
  }
}
