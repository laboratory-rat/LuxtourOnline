

var app = angular.module("TourApp", ['ngAnimate', 'ngFileUpload']);

app.controller('TourCtrl', ['$scope', '$http', 'Upload', '$window', function ($scope, $http, Upload, $window) {

    $scope.Data = '';
    $scope.ImageLoading = false;
    $scope.Lang = '';

    $scope.SaveChanges = function () {

        if ($scope.Data != undefined && $scope.Data != null)
        {
            $http.post('/Tour/SaveChangesJson', $scope.Data)
            .success(function (response) {
                if (response == null | response == undefined)
                {
                    alert("Server Error");
                }
                else if (response.Result == 'error')
                {
                    alert(response.Data);
                }
                else 
                {
                    alert("Success");

                    window.onbeforeunload = null;
                    $window.location.href = "/Tour";
                }

            })
        }
    };

    $scope.LoadImage = function (file) {

        $scope.ImageLoading = true;
        
        if ($scope.Data.Image != null && $scope.Data.Image.IsTmp)
        {
            $http.post('/Image/RemoveImageJson', { id: $scope.Data.Image.Id });
        }

        Upload.upload({
            url: '/Image/SaveImageJson',
            data:
                {
                    file: file,
                }
        })
        .then(function (resp) {
            var r = resp.data;

            if (r.Result === "success") {
                $scope.Data.Image = r.Data;
            }
            else if (r.Result === "error") {
                alert(r.Data);
            }
            else {
                alert("Unknow error.");
            }

            $scope.ImageLoading = false;
        },
        function (resp) {
            alert("Error. Can't load image");
            $scope.ImageLoading = false;
        }, function (evt) {

        });
    };

    $scope.DisplayImage = function () {
        if ($scope.Data == undefined || $scope.Data == null || $scope.Data.Image == null || $scope.Data.Image.Url == "")
            return "http://Placehold.it/600x600";

        return $scope.Data.Image.Url;
    };

    $scope.DisplayTitle = function()
    {
        if ($scope.Data == null)
            return "Empty";

        var title = "";

        for(var i = 0; i < $scope.Data.Descriptions.length; i++)
        {
            var l = $scope.Data.Descriptions[i].Lang;

            if (l == $scope.Lang)
            {
                title = $scope.Data.Descriptions[i].Title;
                break;
            }
        }

        if (title = "")
            return "New Tour";

        return title;
    }

    $scope.init = function(data)
    {
        $scope.Data = data;
    }
    

}]);