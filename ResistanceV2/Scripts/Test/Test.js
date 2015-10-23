
(function () {

    var App = angular.module('RSSFeedApp', []);

    App.controller("FeedCtrl", ['$scope', '$http', 'FeedService', function ($scope, $http, Feed) {
        $scope.loadButonText = "Load";
        $http.get(' http://tecnotree-7.0x10.info/api/tecnotree?type=json&query=list_feed')
          .success(function (data, status, headers) {
              $scope.intialFeed = data.feed;
              var category = [];
              for (var i = 0; i < data.feed.length; i++) {
                  if (category.indexOf(data.feed[i].category) == -1) {
                      category.push(data.feed[i].category)
                  }
              }
              $scope.categories = category;
              $scope.test1 = "abcd";
          })
          .error(function (data, status, headers) {
              $scope.SaveDataResult = "Error occured where syncing the changes with the Server";
              alert($scope.SaveDataResult);
          });
        $scope.loadFeed = function (e) {
            Feed.parseFeed($scope.feedSrc).then(function (res) {
                $scope.loadButonText = angular.element(e.target).text();
                $scope.feeds = res.data.responseData.feed.entries;
            });
        }
    }]);

    App.factory('FeedService', ['$http', function ($http) {
        return {
            parseFeed: function (url) {
                return $http.jsonp(' http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=5&callback=JSON_CALLBACK&q=' + encodeURIComponent(url));
            }
        }
    }]);


})();