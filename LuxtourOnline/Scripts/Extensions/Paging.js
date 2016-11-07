(function() {
    'use strict';

    var paging = angular.module('paging', []);

    paging.directive('paging', Paging);

    function Paging () {
        // Usage:
        //     <Paging></Paging>
        // Creates:
        // 
        var directive = {



            link: link,

            restrict: 'E',
            replace: 'false',
            priority: 1,
            template: '<a ng-repeat="i in GetPages() track by $index"  ng-class="GetClass(i)" >{{i}}</a>',


        };

        return directive;

        function link(scope, element, attrs) {
            scope.page = attrs.page;
            scope.current = attrs.current;
            scope.total = parseInt(attrs.total);

            scope.GetClass = function (i) {
                if(i === scope.current)
                    return "mdl-button mdl-js-button mdl-button--raised mdl-button--primary";
                return "mdl-button mdl-js-button";
            };

            scope.GetPages = function () {
                if (scope.total === undefined)
                    return new Array(1);

                var arr = [];

                for (var i = scope.total; i > 0; i--)
                {
                    arr.push(i);
                }

                return arr;
            };


        }
    }

})();