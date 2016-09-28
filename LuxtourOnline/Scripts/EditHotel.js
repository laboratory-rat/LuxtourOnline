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


    var app = angular.module('EditHotel', ['ngFileUpload']);

    app.controller('EditHotelCtrl', ['$scope', '$http', 'Upload', '$window', function ($scope, $http, Upload, $window) {
        $scope.Data = {};
        $scope.Id;

        $scope.LoadData = function (id) {
            $scope.Id = id;

            $http.get('/Manager/GetHotelEdit/' + $scope.Id)
            .success(function (data) {
                console.log(data)
                $scope.Data = data;
            });
        };

        $scope.UploadData = function () {
            $http.post('/Manager/EditHotel', $scope.Data)
            .success(function (data) {
                if (data == 'success')
                    $window.location.href = "/Manager/HotelsList";
                else
                    alert(data);
            });
        };

        // #region Image

        $scope.uploadFiles = function (files) {
            for (var i = 0; i < files.length; i++) {

                Upload.upload({
                    url: '/Manager/UploadImage',
                    data: { file: files[i] }
                }).then(function (resp) {
                    $scope.Data.Images.push(resp.data.image);
                }, function (resp) {

                }, function (evt) {

                });
            }
        }

        $scope.MoveImage = function (img, vector) {
            var i = $scope.Data.Images.indexOf(img);

            if (vector == "up" && i > 0)
            {


                $scope.Data.Images.move(i, i - 1);
            }
            else if (vector == "down" && i < $scope.Data.Images.length - 1)
            {


                $scope.Data.Images.move(i, i + 1);
            }
        };

        $scope.RemoveImage = function (image) {
            if (image.New) {
                $http.post("/Manager/RemoveTmpImage", image);
            }

            var i = $scope.Data.Images.indexOf(image);
            $scope.Data.Images.splice(i, 1);
        };

        // #endregion

        // #region Features

        $scope.AddFeature = function (description) {
            description.Features.push({ Id: $scope.RandomId() });
        };

        $scope.RemoveFeature = function (description, feature) {
            var i = description.Features.indexOf(feature);
            description.Features.splice(i, 1);
        };

        $scope.MoveFeature = function (description, feature, vector) {
            var i = description.Features.indexOf(feature);

            if (vector == "up" && i > 0)
                description.Features.move(i, i - 1);
            else if (vector == "down" && i < description.Features.length - 1)
                description.Features.move(i, i + 1);
        };
        // #endregion


        // #region Elements

        $scope.AddElement = function (feature, type) {
            if (type == "paid")
                feature.Paid.push({});
            else
                feature.Free.push({});
        };

        $scope.RemoveElement = function (feature, element, type) {
            var i, f;

            if (type == "paid")
                f = feature.Paid;
            else
                f = feature.Free;

            i = f.indexOf(element);
            f.splice(i, 1);
        };

        $scope.MoveElement = function (feature, element, type, vector) {
            var i, f;

            if (type == "paid")
                f = feature.Paid;
            else
                f = feature.Free;

            i = f.indexOf(element);

            if (vector == "up" && i > 0) {
                f.move(i, i - 1);
            }
            else if (vector == "down" && i < f.length - 1) {
                f.move(i, i + 1);
            }
        };

        // #endregion


        $scope.GetGlyph = function (ico) {
            return "glyphicon glyphicon-" + ico;
        };

        $scope.RandomId = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (var i = 0; i < 10; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        };

    }]);

})();