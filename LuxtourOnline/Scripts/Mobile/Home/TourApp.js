app = angular.module('TourApp', ['ngAnimate']);

app.controller('TourCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.Data = [];

    $scope.PerPage = 10;
    $scope.Page = 1;
    $scope.Next = true;

    $scope.Lang = 'uk';

    $scope.IsLoading = false;

    $scope.ActiveTour = null;

    $scope.SetActiveTour = function (tour) {
        console.log('Tour');
        console.log(tour);

        if (tour != undefined)
        {
            $scope.ActiveTour = tour;
        }
    };

    $scope.LoadData = function()
    {
        if($scope.Next)
        {
            $scope.IsLoading = true;

            $http({
                url: '/api/Tour/',
                method: 'GET',
                params: {
                    count: $scope.PerPage,
                    offset: ($scope.Page - 1),
                    language: $scope.Lang
                }
            })
            .success(function (response) {
                if (response === undefined || response.length === 0) {
                    $scope.Next = false;
                }
                else {
                    $scope.Page += 1;
                    for (var i = 0; i < response.length; i++) {
                        $scope.Data.push(response[i]);
                    }
                }
                $scope.IsLoading = false;

            })
            .error(function (response) {
                $scope.Next = false;
                $scope.IsLoading = false;
            })
        }
    }

}]);