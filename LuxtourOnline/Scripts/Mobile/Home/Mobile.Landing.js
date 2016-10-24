var app = angular.module('LandApp', ['ngAnimate', 'angular-carousel']);

app.controller("LandCtrl", ['$scope', '$http', function ($scope, $http) {
    
    $scope.SliderArray = ['', '', '', ''];

    $scope.GetArray = function()
    {
        return new Array(4);
    }
}]);