(function () {
    'use strict';

    Array.prototype.move = function (old_index, new_index) {
        if (new_index >= this.length) {
            var k = new_index - this.length;
            while ((k--) + 1) {
                this.push(undefined);
            }
        }
        this.splice(new_index, 0, this.splice(old_index, 1)[0]);
        return this; // for testing purposes
    };


    var app = angular.module('EditHotel', []);

    app.controller('EditHotelCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.Data = {};
        $scope.Id;

        $scope.LoadData = function (id) {
            $scope.Id = id;

            $http.get('/Manager/GetHotelEdit/' + $scope.Id)
            .success(function (data) {
                console.log(data)
                $scope.Data = data;
            });
        }

        $scope.GetGlyph = function(ico)
        {
            return "glyphicon gliphicon-" + ico;
        }

    }]);

})();