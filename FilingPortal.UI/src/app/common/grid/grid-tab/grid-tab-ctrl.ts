import { ViewChild, Input } from '@angular/core';
import { sortRows } from '@custom/ngx-datatable/utils';

import { Column } from '../models/column';
import { Page } from '../models/page';
import { of } from 'rxjs';

export class GridTabCtrl {
    @Input() title: string;
    @Input() errorsGroup: string;

    public data: Array<any> = [];
    public rows: Array<any> = [];
    public columns: Array<Column> = [];
    public paginationOptions: Page = new Page();
    public maxCols: number = 10;
    public sorts: Array<any> = [];

    @ViewChild('gridTable') gridTable: any;

    constructor(
    ) {}

    setPageNumber(newPageNumber) {
        this.paginationOptions.pageNumber = newPageNumber;
        this.getPage();
    }

    setSort(pageInfo) {
        this.sorts = pageInfo.defSortGrid;
        this.data = sortRows(this.data, this.columns, this.sorts);
        this.getPage();
    }

    setPageSize(pageSize) {
        this.paginationOptions.size = pageSize;
        this.paginationOptions.pageNumber = 1;
        this.getPage();
    }

    getPage() {
        const pageSize = this.paginationOptions.size;
        const from = (this.paginationOptions.pageNumber - 1) * pageSize;

        this.rows = this.data.slice(from, from + pageSize);
        this.paginationOptions.rowsOnPage = this.rows.length;
    }

    setDefaultSorting() {
        const column = this.columns.find(item => item.isDefaultSort);
        if (!column) {
            return;
        }
        const newSort = {
            prop: column.prop,
            dir:  'asc'
        };
        this.sorts = [newSort];
        this.data = sortRows(this.data, this.columns, this.sorts);
    }

    getDataTab() {
      return of([]);
    }

    getData() {
        this.getDataTab().subscribe(data => this.setRows(data));
    }

    setRows(data) {
        this.data = data;
        this.paginationOptions.filteredRows = this.data.length;
        this.setDefaultSorting();
        this.getPage();
    }


    sortFormattedNumbers(a, b) {
        const toNumber = (value) => Number(value.toString().replace(/\,/g, ''));
        if (toNumber(a) > toNumber(b)) {
            return 1;
        } else if (toNumber(a) < toNumber(b)) {
            return -1;
        }
        return 0;
    }
}
