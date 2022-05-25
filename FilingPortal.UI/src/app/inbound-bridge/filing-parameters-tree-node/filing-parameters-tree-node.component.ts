import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TreeNode, InboundRecordParameter, FieldsFilterSettings } from '@inbound/models';
import { FilingParametersService } from '@inbound/services';

@Component({
  selector: 'lxft-filing-parameters-tree-node',
  templateUrl: './filing-parameters-tree-node.component.html'
})
export class FilingParametersTreeNodeComponent implements OnInit {
  @Input() node: TreeNode<InboundRecordParameter>;
  @Input() protected filterSettings: FieldsFilterSettings;
  @Input() viewMode: boolean = false;
  @Input() index: number;
  @Output() onChange: EventEmitter<any> = new EventEmitter();
  @Output() onResize: EventEmitter<any> = new EventEmitter();

  actions = null;

  constructor(protected filingService: FilingParametersService) {}

  ngOnInit() {
    this.actions = this.getAddAction();
  }

  getAddAction(): { name: string; section: string; parentId: number }[] {
    const actions: { name: string; section: string; parentId: number }[] = [];
    this.node.children.forEach(x => {
      if (x.actions['Add'] && actions.findIndex(a => a.name !== x.name) === -1) {
        actions.push({
          name: `Add ${x.title}`,
          section: x.name,
          parentId: x.parentId
        });
      }
    });
    return actions;
  }

  add(section: string, parentId: number) {
    this.filingService.add(section, parentId);
    this.onResize.emit();
  }

  delete(section: string, recordId: number) {
    this.filingService.delete(section, recordId);
    this.onResize.emit();
  }

  isNodeVisible(node: TreeNode<any>): boolean {
    return node && (node.data.length > 0 || node.children.some(child => this.isNodeVisible(child)));
  }

  onNodeChange($event: any): void {
    this.onChange.emit($event);
  }

  onChildSizeChange($event: any): void {
    this.onResize.emit($event);
  }

  onFieldBlur(field: InboundRecordParameter) {
    this.filingService.recalculateFields();
  }
}
