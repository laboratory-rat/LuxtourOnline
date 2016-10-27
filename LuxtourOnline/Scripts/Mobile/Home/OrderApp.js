
var app = angular.module('OrderApp', ['ngSanitize', 'ngAnimate', 'angular-carousel']);

app.controller('OrderCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.Data = null;
    $scope.TourId = '';
    $scope.Lang = '';

    $scope.TodayDate = new Date();
    $scope.HotelLoading = false;

    $scope.ListData = [];

    $scope.GeneralInfoShow = false;

    $scope.UploadData = {
        fromCity: '',
        dateFrom: '',
    };

    $scope.SetModalData = function(data)
    {
        if (data !== undefined && data !== null)
        {
            $scope.ModalData = data;
            showDialog();
        }
    }

    $scope.TapList = function(f)
    {
        var index = $scope.ListData.data.indexOf(f);

        if (index > -1)
        {
            $scope.ListData.state[index] = !$scope.ListData.state[index];
        }
    }

    $scope.IsListActive = function(f)
    {
        var index = $scope.ListData.data.indexOf(f);

        if (index <= -1)
            return false;

        return $scope.ListData.state[index];
    }


    $scope.ActiveHotel = null;

    $scope.GetArrayFromInt = function(int)
    {
        return new Array(int);
    }

    $scope.SetHotel = function(h)
    {
        if (h != undefined && h.avaliableApartments)
            $scope.ActiveHotel = h;
    }

    $scope.LoadData = function()
    {
        $http({
            method: 'GET',
            url: '/Home/OrderTourJson',
            params: {
                id: $scope.TourId,
            }
        })
        .success(function (response) {
            if (response.result === 'success')
            {
                $scope.Data = response.data;
            }
            else
            {
                alert(response.data);
                location.href = '/';
            }
        })
        .error(function (response) {
            alert(response);

            location.href = "/";
        })
    }

    $scope.LoadHotel = function(hotel)
    {
        if (!hotel.avaliableApartments)
            return;

        var id = hotel.id;

        $scope.HotelLoading = true;

        $http({
            method: 'GET',
            url: '/Home/OrderHotelJson',
            params: {id: id},
        })
        .success(function (response) {
            if (response.result === 'success')
            {
                $scope.ActiveHotel = response.data;
                
                var l = {data: [], state: []};

                for(var i = 0; i < response.data.features.length; i++)
                {
                    l.data.push(response.data.features[i]);
                    l.state.push(false);
                }
                
                $scope.ListData = l;
            }
            else 
            {
                alert(response.data);
            }

            $scope.HotelLoading = false;
        })
        .error(function (response) {

        })
    }
}]);