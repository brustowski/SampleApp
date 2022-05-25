import { EventEmitter, TemplateRef, ChangeDetectorRef } from '@angular/core';
export declare class DatatableRowDetailDirective {
    private cdr;
    _rowHeight: number | ((row?: any, index?: number) => number);
    /**
     * The detail row height is required especially
     * when virtual scroll is enabled.
     */
    rowHeight: number | ((row?: any, index?: number) => number);
    template: TemplateRef<any>;
    /**
     * Row detail row visbility was toggled.
     */
    toggle: EventEmitter<any>;
    constructor(cdr: ChangeDetectorRef);
    /**
     * Toggle the expansion of the row
     */
    toggleExpandRow(row: any): void;
    /**
     * API method to expand all the rows.
     */
    expandAllRows(): void;
    /**
     * API method to collapse all the rows.
     */
    collapseAllRows(): void;
}
