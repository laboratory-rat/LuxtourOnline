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


(function () {
    'use strict';

    var app = angular.module('AddHotel', ['ngFileUpload']);

    app.controller('HotelCtrl', ['$scope','$http', '$window', 'Upload', function ($scope, $http, $window, Upload) {

        $scope.Data = {
            Title: "Hotel title",
            Avaliable: false,
            Rate: 5,
            Images: [],

            DescriptionEn:
            {
                Description: "Some Description (en)",
                
                Features:
                [
                        {
                            Id: "asdwfewefs",
                            Title: "Card title",
                            Description: "Card description",
                            Gliph: "ok",

                            Free: [{Title: "F_e title", Glyph: "Glyph"}],
                            Paid: [{Title: "Paind element", Glyph: "Gliph" }]
                        }
                ],
            },

            DescriptionUk:
            {
                Description: "Some Description (uk)",

                Features:
                [
                        {
                            Id: "asaDGefgerrgfs",
                            Title: "Card title",
                            Description: "Card description",
                            Gliph: "ok",


                            Free: [{ Title: "F_e title", Glyph: "Glyph" }],
                            Paid: [{ Title: "Paind element", Glyph: "Gliph" }]
                        }
                ],
            },

            DescriptionRu:
            {
                Description: "Some Description (ru)",

                Features:
                [
                        {
                            Id: "pokprgfs",
                            Title: "Card title",
                            Description: "Card description",
                            Gliph: "ok",

                            Free: [{ Title: "F_e title", Glyph: "Glyph" }],
                            Paid: [{ Title: "Paind element", Glyph: "Gliph" }]
                        }
                ],
            },
        }

        $scope.Collapse = function (id) {
            var target = angular.element('#id');

            console.log(target);

            if (target.hasClass('collapse'))
            {
                target.removeClass('collapse');
            }
            else {
                target.addClass('collapse');
            }
        };

        $scope.uploadFiles = function (files)
        {
            for (var i = 0; i < files.length; i++) 
            {
                console.log(files[i]);

                Upload.upload({
                    url: 'UploadImage',
                    data: { file: files[i] }
                }).then(function (resp) {
                    $scope.Data.Images.push(resp.data.url);
                }, function (resp) {

                }, function (evt) {

                });
            }
        }

        $scope.RemoveImage = function(image)
        {
            var i = $scope.Data.Images.indexOf(image);
            $scope.Data.Images.splice(i, 1);
        }


        $scope.CardTitle = function(title)
        {
            if (title != null && title != "")
                return title;
            return "Card title";
        }

        $scope.AddCard = function (lang) {
            var n = {
                Title: "Card title",
                Description: "Card description",
                Gliph: "Gliph - ico",
                Id: $scope.RandomId(),

                Free: [{ Title: "F_e title", Glyph: "Glyph" }],
                Paid: [{ Title: "Paind element", Glyph: "Gliph" }]
            };

            var nn = {};

            switch (lang) {
                case "en":
                    $scope.Data.DescriptionEn.Features.push(n);
                    break;
                case "uk":
                    $scope.Data.DescriptionUk.Features.push(n);
                    break;
                case "ru":
                    $scope.Data.DescriptionRu.Features.push(n);
                    break;
            }
        };

        $scope.RemoveCard = function (lang, element)
        {
            switch(lang)
            {
                case "en":
                    var i = $scope.Data.DescriptionEn.Features.indexOf(element);
                    $scope.Data.DescriptionEn.Features.splice(i, 1);
                    break;

                case "uk":
                    var i = $scope.Data.DescriptionUk.Features.indexOf(element);
                    $scope.Data.DescriptionUk.Features.splice(i, 1);
                    break;

                case "ru":
                    var i = $scope.Data.DescriptionRu.Features.indexOf(element);
                    $scope.Data.DescriptionRu.Features.splice(i, 1);
                    break;
            }
        }

        $scope.RemoveFree = function (f, ff, lang) {
            switch (lang) {
                case "en":
                    var i = $scope.Data.DescriptionEn.Features.indexOf(f);
                    var j = $scope.Data.DescriptionEn.Features[i].Free.indexOf(ff);

                    $scope.Data.DescriptionEn.Features[i].Free.splice(j, 1);
                    break;
                case "uk":
                    var i = $scope.Data.DescriptionUk.Features.indexOf(f);
                    var j = $scope.Data.DescriptionUk.Features[i].Free.indexOf(ff);

                    $scope.Data.DescriptionUk.Features[i].Free.splice(j, 1);
                    break;
                case "ru":
                    var i = $scope.Data.DescriptionRu.Features.indexOf(f);
                    var j = $scope.Data.DescriptionRu.Features[i].Free.indexOf(ff);

                    $scope.Data.DescriptionRu.Features[i].Free.splice(j, 1);
                    break;
            }
        };

        $scope.RemovePaid = function(f, ff, lang)
        {
            switch (lang) {
                case "en":
                    var i = $scope.Data.DescriptionEn.Features.indexOf(f);
                    var j = $scope.Data.DescriptionEn.Features[i].Paid.indexOf(ff);

                    $scope.Data.DescriptionEn.Features[i].Paid.splice(j, 1);
                    break;
                case "uk":
                    var i = $scope.Data.DescriptionUk.Features.indexOf(f);
                    var j = $scope.Data.DescriptionUk.Features[i].Paid.indexOf(ff);

                    $scope.Data.DescriptionUk.Features[i].Paid.splice(j, 1);
                    break;
                case "ru":
                    var i = $scope.Data.DescriptionRu.Features.indexOf(f);
                    var j = $scope.Data.DescriptionRu.Features[i].Paid.indexOf(ff);

                    $scope.Data.DescriptionRu.Features[i].Paid.splice(j, 1);
                    break;
            }
        };

        $scope.AddElement = function(f,type, lang)
        {
            var n = {};

            if (type == "free") {
                switch (lang) {
                    case "en":
                        var i = $scope.Data.DescriptionEn.Features.indexOf(f);
                        $scope.Data.DescriptionEn.Features[i].Free.push(n);
                        break;
                    case "uk":
                        var i = $scope.Data.DescriptionUk.Features.indexOf(f);
                        $scope.Data.DescriptionUk.Features[i].Free.push(n);
                        break;
                    case "ru":
                        var i = $scope.Data.DescriptionRu.Features.indexOf(f);
                        $scope.Data.DescriptionRu.Features[i].Free.push(n);
                        break;
                }
            }
            else {
                switch (lang) {
                    case "en":
                        var i = $scope.Data.DescriptionEn.Features.indexOf(f);
                        $scope.Data.DescriptionEn.Features[i].Paid.push(n);
                        break;
                    case "uk":
                        var i = $scope.Data.DescriptionUk.Features.indexOf(f);
                        $scope.Data.DescriptionUk.Features[i].Paid.push(n);
                        break;
                    case "ru":
                        var i = $scope.Data.DescriptionRu.Features.indexOf(f);
                        $scope.Data.DescriptionRu.Features[i].Paid.push(n);
                        break;
                }
            }
        };

        $scope.MoveElement = function (f, ff, type, vector) {

            var j;
            var data;

            if (type == 'free')
            {
                j = f.Free.indexOf(ff);
                data = f.Free;
            }
            else
            {
                j = f.Paid.indexOf(ff);
                data = f.Paid;
            }


            if (vector == "up" && j >= 1) {
                data.move(j, j - 1);
            }
            else if (j + 1 < data.length) {
                data.move(j, j + 1);
            }   
        };

        $scope.MoveCard = function (f, vector, lang) {

            var j;
            var data;

            switch(lang)
            {
                case "en":
                    data = $scope.Data.DescriptionEn.Features;
                    break;
                case "uk":
                    data = $scope.Data.DescriptionUk.Features;
                    break;
                case "ru":
                    data = $scope.Data.DescriptionRu.Features;
                    break;
            }

            j = data.indexOf(f);

            if (vector == "up" && j >= 1) {
                data.move(j, j - 1);
            }
            else if (j + 1 < data.length) {
                data.move(j, j + 1);
            }
        };

        $scope.RandomId = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (var i = 0; i < 10; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        };

        $scope.CreateGlyph = function (gl) {
            return "glyphicon glyphicon-" + gl;
        };

        $scope.UploadData = function () {
            $http.post('CreateHotel', $scope.Data, {})
            .success(function () {
                $window.location.href = "HotelsList"
            })
            .error(function () {
            });
        };

    }]);

})();