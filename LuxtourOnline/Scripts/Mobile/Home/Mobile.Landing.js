var app = angular.module('LandApp', ['ngAnimate', 'angular-carousel']);

app.controller("LandCtrl", ['$scope', '$http', function ($scope, $http) {
    
    $scope.SliderArray = ['', '', '', ''];

    $scope.sliderContainer = 0;

    $scope.GetArray = function()
    {
        return new Array(4);
    }
}]);