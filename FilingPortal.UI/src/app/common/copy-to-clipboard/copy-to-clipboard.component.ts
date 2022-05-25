import { Component, Input } from '@angular/core';
import { NgbPopoverConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-copy-to-clipboard',
  templateUrl: './copy-to-clipboard.component.html',
  providers: [NgbPopoverConfig]
})
export class CopyToClipboardComponent {
  @Input()
  public ctcValue: string = '';

  constructor(popoverConfig: NgbPopoverConfig) {
    popoverConfig.container = 'body';
  }
}
