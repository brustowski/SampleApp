import { Component, Input, OnInit } from '@angular/core';

import { GridPageApiService } from '../services/grid-page-api.service';

import { Page } from '../models/page';

@Component({
    selector: 'lxft-grid-count-results',
    templateUrl: './grid-count-results.component.html'
})
export class GridCountResultsComponent implements OnInit {
    @Input() pageOptions: Page;

    private totalRows: number = 0;
    private isTotalsLoader: boolean = false;

    constructor(
        private api: GridPageApiService
    ) {
        this.isTotalsLoader = true;
    }

    ngOnInit() {
        if (this.pageOptions.showTotalRows) {
            this.getTotalRows();
        }
    }

    getFromItem() {
        return this.pageOptions.size * (this.pageOptions.pageNumber - 1) + 1;
    }

    getToItem() {
        const from = this.getFromItem() + this.pageOptions.rowsOnPage - 1;

        return from;
    }

    getTotalRows() {
        this.isTotalsLoader = true;
        this.api.getTotalRows(this.pageOptions.pathForApi)
            .subscribe((data) => {
                this.totalRows = data.RowCount;
                this.isTotalsLoader = false;
            });
    }
}
