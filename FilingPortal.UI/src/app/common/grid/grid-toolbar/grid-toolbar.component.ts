import { Component, Input, Output, EventEmitter } from '@angular/core';

import { Page } from '../models/page';

@Component({
    selector: 'lxft-grid-toolbar',
    templateUrl: './grid-toolbar.component.html'
})
export class GridToolbarComponent {
    @Input() pageOptions: Page;
    @Input() isLoading: boolean;
    @Input() disabled = false;
    @Output() setPageNumber: EventEmitter<number> = new EventEmitter();
    @Output() setPageSize: EventEmitter<number> = new EventEmitter();

    constructor() { }

    setSize(newSize) {
        this.setPageSize.emit(newSize);
    }

    public get isDisabledDropButton(): boolean {
        return this.isLoading || this.disabled;

    }
    public get isDisabledPrevButton(): boolean {
        return this.isLoading || this.disabled || this.pageOptions.pageNumber <= 1;
    }

    public get isDisabledNextButton(): boolean {
        return this.isLoading || this.disabled || this.getPagesCount() <= this.pageOptions.pageNumber;
    }

    getPagesCount() {
        return Math.ceil(this.pageOptions.filteredRows / this.pageOptions.size);
    }

    previousPage() {
        const newPageNumber = this.pageOptions.pageNumber - 1;
        this.setPageNumber.emit(newPageNumber);
    }

    nextPage() {
        const newPageNumber = this.pageOptions.pageNumber + 1;
        this.setPageNumber.emit(newPageNumber);
    }
}
