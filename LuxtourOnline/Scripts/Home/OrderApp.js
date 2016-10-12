var app = angular.module("OrderApp", ["ngAnimate"]);
app.controller("OrderCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.TourData = null;
    $scope.TourId = null;

    $scope.LoadTour = function () {
        if ($scope.TourId != null) {
            $http({
                method: 'GET',
                url: '/Home/OrderTourJson',
                params: {
                    id: $scope.TourId,
                },
            })
            .success(function (response) {
                if (response.result == "success")
                {
                    $scope.TourData = response.data;
                }
            });
        }
    };
}]);