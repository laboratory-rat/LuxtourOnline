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


var app = angular.module("OrderApp", ['ngAnimate', 'ngTouch', 'angular-carousel', 'ngSanitize', 'ng-file-input']);

app.filter('to_trusted', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);

app.directive("fileread", [function () {
    return {
        scope: {
            fileread: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                var reader = new FileReader();
                reader.onload = function (loadEvent) {
                    scope.$apply(function () {
                        scope.fileread = loadEvent.target.result;
                    });
                }
                reader.readAsDataURL(changeEvent.target.files[0]);
            });
        }
    }
}]);

app.controller("OrderCtrl", ['$scope', '$http', function ($scope, $http) {
    $scope.TourData = null;
    $scope.TourId = null;
    $scope.Lang = null;


    $scope.ActiveHotel = null;
    $scope.HotelList = [];

    $scope.TmpAA = null;

    $scope.ActiveApartment = null;
    $scope.FromDate = null;
    $scope.TodayDate = new Date();

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
    $scope.GetLength = function (element)
    {
        if (element == undefined || element == null)
            return 0;
        return element.length;
    }

    $scope.IsValidSecond = function()
    {
        if ($scope.TourData === null)
            return false;

        if ($scope.ActiveHotel === null)
            return false;

        if ($scope.ActiveHotel.aa === null)
            return false;

        if ($scope.UploadData.dateFrom === '' || $scope.UploadData.fromCity === '' || $scope.UploadData.city === '' || $scope.UploadData.email === '' || $scope.UploadData.phone === '')
            return false;

        if ($scope.UploadData.customers !== null && $scope.UploadData.customers.length > 0) {
            for (var i = 0; i < $scope.UploadData.customers.length; i++) {
                var c = $scope.UploadData.customers[i];

                if (c.fullName === '' || c.bithday === '')
                    return false;

                if (!c.isChild) {

                    if (c.passportIsFiles) {
                        if (c.passportFiles.length < 1)
                            return false;
                    }
                    else {
                        if (c.passportData === '' ||
                             c.passportNumber === '' ||
                             c.passportUntil === '' ||
                             c.passportFrom === '' ||
                            c.countryFrom === '' ||
                             c.countryLive === '')
                            return false;
                    }
                }
            }
        }
        else {
            return false;
        }

        return true;
    }

    $scope.IsValid = function () {

        if ($scope.TourData === null)
            return false;

        if ($scope.ActiveHotel === null)
            return false;

        if ($scope.ActiveHotel.aa === null)
            return false;

        if ($scope.UploadData.dateFrom === '' || $scope.UploadData.fromCity === '' || $scope.UploadData.city === '' || $scope.UploadData.email === '' || $scope.UploadData.phone === '')
            return false;

        if ($scope.UploadData.customers !== null && $scope.UploadData.customers.length > 0) {
            for (var i = 0; i < $scope.UploadData.customers.length; i++) {
                var c = $scope.UploadData.customers[i];

                if ( c.fullName === '' || c.bithday === '')
                    return false;

                if (!c.isChild) {

                    if (c.passportIsFiles) {
                        if (c.passportFiles.length < 1)
                            return false;
                    }
                    else {
                        if ( c.passportData === '' ||
                             c.passportNumber === '' ||
                             c.passportUntil === '' ||
                             c.passportFrom === '' ||
                            c.countryFrom === '' ||
                             c.countryLive === '')
                            return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    };

    $scope.IsLoadingHotel = false;

    $scope.SendData = function()
    {
        var data = $scope.UploadData;
        data.HotelId = $scope.ActiveHotel.id;
        data.TourId = $scope.TourId;
        data.ApartmentID = $scope.ActiveHotel.aa.id;

        $http.post('/'+$scope.Lang+'/Home/CreateOrder', data)
        .success(function (response) {
            if (response.result == "error")
            {
                alert(response.data);
            }
            else
            {
                alert(response.data)
            }
        });
        
        
    }

    $scope.AddCustomer = function(){$scope.AddCustomer(false);}

    $scope.AddCustomer = function(child)
    {
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

    $scope.ClearCustomers = function()
    {
        $scope.UploadData.customers = [];

        if ($scope.ActiveHotel.aa != null)
        {
            for(var i = 0; i < $scope.ActiveHotel.aa.adult + $scope.ActiveHotel.aa.child; i++)
            {
                if (i < $scope.ActiveHotel.aa.adult)
                    $scope.AddCustomer(false);
                else
                    $scope.AddCustomer(true);
            }
        }
    }

    $scope.IsNull = function(arr)
    {
        if (arr == undefined)
            return true;

        return arr.length === 0;
    }

    $scope.IsValid = function()
    {
        return true;
    }

    $scope.isStrip = function(hotel)
    {
        var i = $scope.TourData.hotels.indexOf(hotel);
        var s = i % 2;

        return s === 1;
    }

    $scope.GetCustomerIndex = function(customer)
    {
        return $scope.UploadData.customers.indexOf(customer);
    }

    $scope.IsCustomerFirst = function(customer)
    {
        return ($scope.GetCustomerIndex(customer) === 0);
    }

    $scope.ActiveCustomer = '';
    $scope.SetCustomerActive = function(id)
    {
        console.log(id);

        if ($scope.ActiveCustomer !== '')
        {
            angular.element('#' + $scope.ActiveCustomer).removeClass('is-active');
            angular.element('#link-' + $scope.ActiveCustomer).removeClass('is-active');
        }

        $scope.ActiveCustomer = id;

        angular.element('#' + $scope.ActiveCustomer).addClass('is-active');
        angular.element('#link-' + $scope.ActiveCustomer).addClass('is-active');
    }

    $scope.SetHotel = function(hotel)
    {
        var index = -1;
        $scope.ActiveHotel = null;


        for(var i = 0; i < $scope.HotelList.length; i++)
        {
            console.log('index: ' + index);
            console.log('id: ' + $scope.HotelList[i].id);

            if ($scope.HotelList[i].id === hotel.id)
            {
                index = i;
                break;
            }
        }

        if (index !== -1)
        {
            $scope.ActiveHotel = $scope.HotelList[i];
        }
        else 
        {
            $scope.IsLoadingHotel = true;
            var newHotel = null;
            var lang = $scope.Lang;


            $http({
                method: 'GET',
                url: '/'+$scope.Lang+'/Home/OrderHotelJson',
                params: {
                    id: hotel.id,
                },
            })
            .success(function (response) {
                $scope.IsLoadingHotel = false;

                newHotel = response.data;
                newHotel.aa = null;
                $scope.HotelList.push(newHotel);
                $scope.ActiveHotel = newHotel;
            });
        }
    }

    $scope.SetAA = function(hotel, id)
    {
        console.log(hotel);
        console.log(id);

        var index = -1;
        for (var i = 0; i < $scope.TourData.hotels.length; i++)
        {
            console.log($scope.TourData.hotels);

            if (hotel.id === $scope.TourData.hotels[i].id)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return;

        var apartIndex = -1;

        for (var i = 0; i < $scope.TourData.hotels[index].apartments.length; i++)
        {
            if ($scope.TourData.hotels[index].apartments[i].id == id)
            {
                apartIndex = i;
                break;
            }
        }

        if (apartIndex == -1)
            return;

        var apart = $scope.TourData.hotels[index].apartments[apartIndex];

        var hotelIndex = -1;

        for (var i = 0; i < $scope.HotelList.length; i++)
        {
            if (hotel.id == $scope.HotelList[i].id)
            {
                hotelIndex = i;
                break;
            }
        }

        if (hotelIndex == -1)
            return;

        $scope.HotelList[hotelIndex].aa = apart;
        $scope.ClearCustomers();
    }
    
    $scope.UntilDate = function(days)
    {
        if ($scope.UploadData.dateFrom == null)
            return;

        var until = new Date($scope.UploadData.dateFrom);
        until.setDate(until.getDate() + days);
        return until;
    }

    //Loading area

    $scope.LoadTour = function () {
        if ($scope.TourId !== null) {

            $http({
                method: 'GET',
                url: '/'+$scope.Lang+'/Home/OrderTourJson',
                params: {
                    id: $scope.TourId,
                },
            })
            .success(function (response) {
                if (response.result === "success")
                {
                    angular.element('#main-card').css('visibility', 'visible');
                    angular.element('#hotel-table').css('visibility', 'visible');
                    $scope.TourData = response.data;
                }
                else 
                {
                    console.log("Error: " + response.data);
                }
            });
        }
    };
    
}]);