﻿(function () {

    var app = angular.module('app');

    app.service('horsesService', HorsesService);

    function HorsesService(sharedValues, $http, $q) {

       
        this.getHorses = _getHorses;
        this.getHorse = _getHorse;
        this.updateHorse = _updateHorse;
        this.deleteHorse = _deleteHorse;

        function _getHorses(includeDeleted) {
         
            var deferred = $q.defer();
           
            $http.get(sharedValues.apiUrl + 'horses/gethorses' + (includeDeleted ? '/' + includeDeleted : '')).then(function (res) {

                var horses = res.data;
                for (var i in horses) {
                    horses[i].Meta = JSON.parse(horses[i].Meta);
                }
                deferred.resolve(horses);
            });
            return deferred.promise;
        }

        function _getHorse(id) {
          
            var deferred = $q.defer();
            if (id == 0) {
                $http.get(sharedValues.apiUrl + 'horses/newhorse/').then(function (res) {
                    deferred.resolve(res.data);
                });
            }
            else {
                $http.get(sharedValues.apiUrl + 'horses/gethorse/' + id).then(function (res) {
                    var horse = res.data;
                    horse.Meta = angular.fromJson(horse.Meta);
                    horse.Meta.BirthDate = horse.Meta.BirthDate != '' ? new Date(horse.Meta.BirthDate) : null;
                    deferred.resolve(horse);
                });
            }
            return deferred.promise;
        }

        function _updateHorse(horse) {
            var deferred = $q.defer();
            horse.Meta = angular.toJson(horse.Meta);
            $http.post(sharedValues.apiUrl + 'horses/updatehorse', horse).then(function (res) {
                var horse = res.data;
                horse.Meta = angular.fromJson(horse.Meta);
                horse.Meta.BirthDate = horse.Meta.BirthDate != '' ? new Date(horse.Meta.BirthDate) : null;
                deferred.resolve(horse);
            });
            return deferred.promise;
        }

        function _deleteHorse(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'horses/deleteHorse/' + id).then(function (res) {
                deferred.resolve();
            });
            return deferred.promise;
        }

    }

})();