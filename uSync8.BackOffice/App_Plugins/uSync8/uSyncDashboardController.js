﻿/**
 *@ngdoc
 *@name uSync8DashboardController
 *@requires uSync8DashboardService
 * 
 *@description controller for the uSync dashboard
 */

(function () {
    'use strict';

    function uSyncDashboardController($scope,
        uSync8DashboardService,
        uSyncHub) {

        var vm = this;
        vm.loading = true;

        vm.working = false;
        vm.reported = false;
        vm.syncing = false;

        var modes = {
            NONE: 0,
            REPORT: 1,
            IMPORT: 2,
            EXPORT: 3
        };

        vm.runmode = modes.NONE;

        vm.showAll = false;

        vm.settings = {};
        vm.handlers = [];
        vm.status = {};

        // functions 
        vm.report = report;
        vm.exportItems = exportItems;
        vm.importItems = importItems;

        vm.toggleDetails = toggleDetails;
        vm.getTypeName = getTypeName;
        vm.toggleAll = toggleAll;
        vm.countChanges = countChanges;

        // kick it all off
        init();

        ////// public 

        function report() {
            resetStatus(modes.REPORT);

            uSync8DashboardService.report(getClientId())
                .then(function (result) {
                    vm.results = result.data;
                    vm.working = false;
                    vm.reported = true;
                });
        }

        function exportItems() {
            resetStatus(modes.EXPORT);

            uSync8DashboardService.exportItems(getClientId())
                .then(function (result) {
                    vm.results = result.data;
                    vm.working = false;
                    vm.reported = true;
                });
        }

        function importItems(force) {
            resetStatus(modes.IMPORT);

            uSync8DashboardService.importItems(force, getClientId())
                .then(function (result) {
                    vm.results = result.data;
                    vm.working = false;
                    vm.reported = true;
                });
        }

        function toggleDetails(result) {
            result.showDetails = !result.showDetails;
        }


        function getTypeName(typeName) {
            var umbType = typeName.substring(0, typeName.indexOf(','));
            return umbType.substring(umbType.lastIndexOf('.') + 1);
        }

        function toggleAll() {
            vm.showAll = !vm.showAll;
        }

        function countChanges(changes) {
            var count = 0;
            angular.forEach(changes, function (val, key) {
                if (val.Change !== 'NoChange') {
                    count++;
                }
            });

            return count;
        }

        ////// private 

        function init() {

            uSyncHub.initHub(function (hub) {

                vm.hub = hub;

                vm.hub.on('add', function (data) {
                    vm.status = data;
                });

                vm.hub.start();
            });

            getSettings();
            getHandlers();
        }

        function getSettings() {

            uSync8DashboardService.getSettings()
                .then(function (result) {
                    vm.settings = result.data;
                    vm.loading = false;

                });
        }

        function getHandlers() {
            uSync8DashboardService.getHandlers()
                .then(function (result) {
                    vm.handlers = result.data;
                });
        }

        function resetStatus(mode) {
            vm.reported = false;
            vm.working = true;
            vm.runmode = mode;

            vm.status = {
                Percent: 0,
                Message: ""
            };
        }

        function getClientId() {
            if ($.connection !== undefined && $.connection.hub !== undefined) {
               return $.connection.hub.id;
            }

            return "";
        }

    }

    angular.module('umbraco')
        .controller('uSync8DashboardController', uSyncDashboardController);

})();