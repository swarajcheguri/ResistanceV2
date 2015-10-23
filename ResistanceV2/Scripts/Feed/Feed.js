(function () {

    var feedModule = angular.module("FeedModule", ['ui.bootstrap']);
    hub = $.connection.myHub1;
    $.connection.hub.start(); // connect to signalr hub
    feedModule.controller("FeedController", function ($scope, $modal, $rootScope, $http, $uibModal, $log) {
        $scope.broadcasteMessage = "";
            $http.get('/Feed/GetFeed/')
          .success(function (data, status, headers) {
              $scope.messages = data;
          })
          .error(function (data, status, headers) {
              $scope.SaveDataResult = "Error occured while reterving the feed";
              alert($scope.SaveDataResult);
          });
            $scope.CancelRegistration = function () {
                angular.element('#confirmationModal').modal('hide');


            }
            $scope.openModal = function () {
                angular.element('#confirmationModal').modal('show');


            }
            $scope.postMessage = function () {
                var dataObj = {
                    msg: $scope.broadcasteMessage
                };
                $http.post('/Feed/SaveMessage/', dataObj)
              .success(function (data, status, headers) {
                  $scope.broadcasteMessage = "";
              })
              .error(function (data, status, headers) {
                  $scope.SaveDataResult = "Error occured while broadcasting the message";
                  alert($scope.SaveDataResult);
              });
            };
            $scope.messageComment = "";
            $scope.commentMsgId = 0;
            
            $scope.AddComment = function () {
                
            var dataObj = {
                cmt:$scope.messageComment,
                msgid: $scope.msgIdCmt
            };
            $http.post('/Feed/CommentSave/', dataObj)
          .success(function (data, status, headers) {
              $scope.messageComment = "";
          })
          .error(function (data, status, headers) {
              $scope.SaveDataResult = "Error occured while commenting on  the message";
              alert($scope.SaveDataResult);
          });

          
        };
        $scope.showFeed = true;
        $scope.savedMessages = null;
        $scope.saveMessageToUser = function (msgid) {

            var dataObj = {
                msgid: msgid
            };
            $http.post('/Feed/SaveMessageToUser/', dataObj)
            .success(function (data, status, headers) {
                alert("Message has been saved");
            })
            .error(function (data, status, headers) {
                $scope.SaveDataResult = "Error occured while Liking the message";
                alert($scope.SaveDataResult);
            });

        }
        $scope.getSavedMessages = function () {
            $http.get('/Feed/GetSavedMessages/')
            .success(function (data, status, headers) {
                $scope.savedMessages = data;
                $scope.showFeed = false;
            })
            .error(function (data, status, headers) {
                $scope.SaveDataResult = "Error occured while Liking the message";
                alert($scope.SaveDataResult);
            });

        }
        $scope.increment=function(msgid)
        {
            var dataObj = {
                msgid: msgid
            };
            $http.post('/Feed/Like/', dataObj)
            .success(function (data, status, headers) {
            })
            .error(function (data, status, headers) {
                $scope.SaveDataResult = "Error occured while Liking the message";
                alert($scope.SaveDataResult);
            });

        }
        $scope.decrement = function (msgid) {
            var dataObj = {
                msgid: msgid
            };
            $http.post('/Feed/DisLike/', dataObj)
            .success(function (data, status, headers) {
            })
            .error(function (data, status, headers) {
                $scope.SaveDataResult = "Error occured while DisLiking the message";
                alert($scope.SaveDataResult);
            });

        }

        hub.client.NewMessage = function (msg) {
            $scope.messages.push(msg);
            $scope.$apply(); // this is outside of angularjs, so need to apply
        }

        hub.client.NewComment = function (comment) {
            for (var i = 0; i < $scope.messages.length; i++) {
                if ($scope.messages[i].MessageId == comment.MessageId) {
                    $scope.messages[i].Comments.push(comment);
                }
            }

            $scope.$apply(); // this is outside of angularjs, so need to apply
        }

        hub.client.NewLike = function (msg) {
            for(var i=0;i< $scope.messages.length;i++)
            {
                if ($scope.messages[i].MessageId == msg.MessageId) {
                    $scope.messages[i].liked = msg.liked;
                }
            }
            
            $scope.$apply(); // this is outside of angularjs, so need to apply
        }
        hub.client.NewDislike = function (msg) {
            for (var i = 0; i < $scope.messages.length; i++) {
                if ($scope.messages[i].MessageId == msg.MessageId) {
                       $scope.messages[i].disliked = msg.disliked;
                }
            }

            $scope.$apply(); // this is outside of angularjs, so need to apply
        }
       


    });

    //feedModule.controller('AddCommentCtrl', ['$scope', '$http', '$rootScope'
    // , function ($scope, $http, $rootScope,$modalInstance, $location) {
    //     $scope.AddCommentSub = function () {
    //         var dataObj = {
    //             cmt: $scope.messageComment,
    //             msgid: $scope.params.msgId
    //         };
    //         $http.post('/Feed/CommentSave/', dataObj)
    //       .success(function (data, status, headers) {
    //           $scope.messageComment = "";
    //           $modalInstance.close();
    //       })
    //       .error(function (data, status, headers) {
    //           $scope.SaveDataResult = "Error occured while commenting on  the message";
    //           alert($scope.SaveDataResult);
    //       });


    //     };
    // }]);




})();