var app = angular.module('ContactsApp', ['ngAnimate']);

app.run(function () {
    var mdlUpgradeDom = false;
    setInterval(function () {
        if (mdlUpgradeDom) {
            componentHandler.upgradeDom();
            mdlUpgradeDom = false;
        }
    }, 200);

    var observer = new MutationObserver(function () {
        mdlUpgradeDom = true;
    });
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
    /* support <= IE 10
    angular.element(document).bind('DOMNodeInserted', function(e) {
        mdlUpgradeDom = true;
    });
    */
});

app.controller('ContactsCtrl', ['$scope', function ($scope) {

}]);