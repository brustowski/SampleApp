import * as R from 'ramda';

export default class BulkColumnController {

    /* @ngInject */
    constructor(gridBuilderService, $scope) {
        this._gridBuilder = gridBuilderService;
        this._scope = $scope;
    }

    getBulkColumn() {
        return this._gridBuilder.getBulkColumn();
    }

    onUpdateGrid() {
        const isBulk = (item) => {
            if (!item.bulk) {
                this._scope.grid.isBalkAll = false;
            }
        };
        R.map(isBulk, this._scope.grid.options.data);
    }

    onChangeBalkAll(isBulkAll) {
        const bulkAll = (item) => {
            item.bulk = isBulkAll;
        };
        R.map(bulkAll, this._scope.grid.options.data);
    }
}
