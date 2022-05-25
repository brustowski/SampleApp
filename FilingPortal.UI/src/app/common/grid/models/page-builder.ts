import { Page } from './page';

export class PageBuilder {
    model: Page;

    create(): PageBuilder {
        this.model = new Page();
        return this;
    }

    size (value: number): PageBuilder {
        this.model.size  = value;
        return this;
    }

    sizes(value: number[]): PageBuilder {
        this.model.sizes = value;
        return this;
    }

    filteredRows(value: number): PageBuilder {
        this.model.filteredRows = value;
        return this;
    }

    totalPages(value: number): PageBuilder {
        this.model.totalPages = value;
        return this;
    }

    pageNumber(value: number): PageBuilder {
        this.model.pageNumber = value;
        return this;
    }

    rowsOnPage(value: number): PageBuilder {
        this.model.rowsOnPage = value;
        return this;
    }

    pathForApi(value: string): PageBuilder {
        this.model.pathForApi = value;
        return this;
    }

    gridName(value: string): PageBuilder {
        this.model.gridName = value;
        return this;
    }

    filterConfigName(value: string): PageBuilder {
        this.model.filterConfigName = value;
        return this;
    }

    isMatchLoading(value: boolean): PageBuilder {
        this.model.isMatchLoading = value;
        return this;
    }

    title(value: string): PageBuilder {
        this.model.title = value;
        return this;
    }

    permissionEdit(value: string): PageBuilder {
        this.model.permissionEdit = value;
        return this;
    }

    permissionDelete(value: string): PageBuilder {
        this.model.permissionDelete = value;
        return this;
    }

    showTotalRows(value: boolean): PageBuilder {
        this.model.showTotalRows = value;
        return this;
    }

    enableSummaryRows(value: boolean): PageBuilder {
      this.model.enableSummary = value;
      return this;
  }

    build(): Page {
        return this.model;
    }
}
