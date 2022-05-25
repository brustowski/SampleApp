import { Input, Output, EventEmitter, Directive, TemplateRef, ContentChild, ChangeDetectorRef } from '@angular/core';
import { DatatableRowDetailTemplateDirective } from './row-detail-template.directive';

@Directive({ selector: 'ngx-datatable-row-detail' })
export class DatatableRowDetailDirective {
  _rowHeight: number | ((row?: any, index?: number) => number) = 0;

  /**
   * The detail row height is required especially 
   * when virtual scroll is enabled.
   */
  @Input() set rowHeight(value: number | ((row?: any, index?: number) => number)) {
    this._rowHeight = value;
    this.cdr.markForCheck();
  }

  get rowHeight(): number | ((row?: any, index?: number) => number){
    return this._rowHeight;
  }

  @Input()
  @ContentChild(DatatableRowDetailTemplateDirective, { read: TemplateRef }) 
  template: TemplateRef<any>;

  /**
   * Row detail row visbility was toggled.
   */
  @Output() toggle: EventEmitter<any> = new EventEmitter();

  constructor(private cdr: ChangeDetectorRef) { }

  /**
   * Toggle the expansion of the row
   */
  toggleExpandRow(row: any): void {
    this.toggle.emit({
      type: 'row',
      value: row
    });
  }

  /**
   * API method to expand all the rows.
   */
  expandAllRows(): void {
    this.toggle.emit({
      type: 'all',
      value: true
    });
  }

  /**
   * API method to collapse all the rows.
   */
  collapseAllRows(): void {
    this.toggle.emit({
      type: 'all',
      value: false
    });
  }

}
