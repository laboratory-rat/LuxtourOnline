function generateUUID() {
    var d = new Date().getTime();
    if (window.performance && typeof window.performance.now === "function") {
        d += performance.now(); //use high-precision timer if available
    }
    var uuid = 'xxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}


var app = angular.module('OrderApp', ['ngSanitize', 'ngAnimate', 'angular-carousel']);

app.controller('OrderCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.Data = null;
    $scope.TourId = '';
    $scope.Lang = '';

    $scope.TodayDate = new Date();
    $scope.HotelLoading = false;

    $scope.ListData = [];
    $scope.TmpAA;

    $scope.GeneralInfoShow = false;

    $scope.UploadData = {
        fromCity: '',
        dateFrom: '',
    };

    $scope.UploadData = {
        email: '',
        phone: '',
        city: '',
        comments: '',

        fromCity: '',
        dateFrom: '',


        customers: [
            /*{
            id: '',
            isChild: false,
            fullName: '',
            countryFrom: '',
            countryLive: '',
            passportFiles: [],
            passportIsFiles: false,
            bithday: '',
            passportUntil: '',
            passportFrom: '',
            passportNumber: '',
            passportFullName: '',
            }
            */
        ]
    };

    // #region Customer area

    $scope.AddCustomer = function () { $scope.AddCustomer(false); }

    $scope.AddCustomer = function (child) {
        var id = generateUUID();

        $scope.UploadData.customers.push({
            id: id,
            isChild: child,
            fullName: '',
            countryFrom: '',
            countryLive: '',
            passportFiles: '',
            passportIsFiles: false,
            bithday: '',
            passportUntil: '',
            passportFrom: '',
            passportNumber: '',
            passportFullName: '',
        });
    }

    $scope.ClearCustomers = function () {
        $scope.UploadData.customers = [];


        if ($scope.ActiveHotel.aa != null) {
            for (var i = 0; i < $scope.ActiveHotel.aa.adult + $scope.ActiveHotel.aa.child; i++) {
                if (i < $scope.ActiveHotel.aa.adult)
                    $scope.AddCustomer(false);
                else
                    $scope.AddCustomer(true);

                console.log(123);
            }
        }
    }

    $scope.SetAA = function(apartment)
    {
        for (var i = 0; i < $scope.ActiveHotel.apartments.length; i++)
        {

            if ($scope.ActiveHotel.apartments[i].id == apartment)
            {
                $scope.ActiveHotel.aa = $scope.ActiveHotel.apartments[i];
                break;
            }
        }

        console.log($scope.ActiveHotel.aa);

        $scope.ClearCustomers();
    }

    $scope.SwitchCustomer = function (customer) {
        if (customer.show == undefined)
            customer.show = true;
        else {
            customer.show = !customer.show;
        }
    };

    $scope.IsShowCustomer = function(customer)
    {
        if (customer.show === undefined)
            return false;

        return customer.show;
    }

    // #endregion

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