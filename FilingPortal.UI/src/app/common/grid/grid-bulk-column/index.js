import angular from 'angular';
import controller from './bulk.controller';

export default angular
    .module('app.components.grid.bulk-column', [])

    .directive('lxftGridBulkColumn', () => {
        return {
            restrict: 'A',
            require: ['uiGrid', 'lxftGridBulkColumn'],
            link: ($scope, $elm, attrs, ctrls) => {
                const ctrl = ctrls[1];
                $scope.grid = ctrls[0].grid;
                $scope.grid.isBalkAll = false;
                $scope.grid.options.columnDefs.push(ctrl.getBulkColumn());

                $scope.grid.onChangeBulkAll = ctrl.onChangeBalkAll.bind(ctrl);
                $scope.grid.onChangeBulk = ctrl.onUpdateGrid.bind(ctrl);
                $scope.grid.api.core.on.rowsRendered($scope, ctrl.onUpdateGrid.bind(ctrl));

                $scope.grid.api.core.refresh();
            },
            controller
        };
    })

    .name;
