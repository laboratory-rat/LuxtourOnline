var app = angular.module("NewsApp", ['ngAnimate']);

app.controller("NewsCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.Data = null;

    $scope.Count = 10;
    $scope.Page;
    $scope.Last = false;

    $scope.LoadMore = function () {
        $scope.Last = true;

        $scope.Load();
    };

    $scope.Dots = function (array) {
        if (array != undefined) {
            if (array.length > 150)
                return true;
        }

        return false;
    };

    $scope.ShowLike = function(array)
    {
        if (array === undefined)
            return false;

        if (array.length > 0)
            return true;

        return false;
    }


    $scope.Load = function () {
        $http({
            url: "/Home/GetNews",
            method: "GET",
            params: { page: $scope.Page, count: $scope.Count }
        })
        .success(function (response) {
            if ($scope.Data == null)
                $scope.Data = [];

            if (response == undefined)
                return;

            for (i = 0; i < response.length; i++)
            {
                if (response[i].message !== undefined && response[i].message.length > 10)
                    $scope.Data.push(response[i]);
            }

            $scope.Last = false;

        });

        $scope.Page++;
    };



    $scope.GetImage = function (i) {
        return "Background-image: url(" + i.full_picture + ");";
    };
}]);