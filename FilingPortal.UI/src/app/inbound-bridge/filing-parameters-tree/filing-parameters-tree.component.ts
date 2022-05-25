import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TreeNode, FieldsFilterSettings } from '@inbound/models';

@Component({
  selector: 'lxft-filing-parameters-tree',
  templateUrl: './filing-parameters-tree.component.html'
})
export class FilingParametersTreeComponent {

  @Input() node: TreeNode<any>;
  @Input() additionalNode: TreeNode<any>;
  @Input() filterSettings: FieldsFilterSettings;
  @Input() viewMode: boolean = false;
  @Output() onChange: EventEmitter<any> = new EventEmitter();
  @Output() onResize: EventEmitter<any> = new EventEmitter();

  constructor() { }

  onNodeChange($event: any): void {
    this.onChange.emit($event);
  }

  onChildSizeChange($event: any): void {
    this.onResize.emit($event);
  }
}
