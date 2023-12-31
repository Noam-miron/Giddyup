﻿(function () {

    var app = angular.module('app');

    app.service('usersService', UsersService);

    function UsersService(sharedValues, $http, $q) {
        this.getUsers = _getUsers;
        this.getUser = _getUser;
        this.updateUser = _updateUser;
        this.updateUserMultiTables = _updateUserMultiTables;
        this.importUsers = _importUsers;
        this.deleteUser = _deleteUser;
        this.getUserIdByEmail = _getUserIdByEmail;
        this.roles = _roles();
        this.getUserExpensesByUserId = _getUserExpensesByUserId;

        this.getPaymentsByUserId = _getPaymentsByUserId;
        this.getUserUserhorsesByUserId = _getUserUserhorsesByUserId;
        this.getAllFarmsuseruserhorses = _getAllFarmsuseruserhorses;



        
        this.getUserFilesByUserId = _getUserFilesByUserId;
        this.getUserCommitmentsByUserId = _getUserCommitmentsByUserId;
        this.getUserUserMakavByUserId = _getUserUserMakavByUserId;



        this.getAvailablehours = _getAvailablehours;
        this.getTransferData = _getTransferData;

        this.report = _report;
        this.reportHMO = _reportHMO;
        this.reportMaccabi = _reportMaccabi;
        this.reportKlalit = _reportKlalit;
        this.reportDebt = _reportDebt;
        this.getStudents = _getStudents;
        
        
        

        function _getUsers(role, includeDeleted) {
            var deferred = $q.defer();
          
            $http.get(sharedValues.apiUrl + 'users/getusers' + (role ? '/' + role : '') + (includeDeleted ? '/' + includeDeleted : '')).then(function (res) {
              
                var users = res.data;
                deferred.resolve(users);
              
            });

            
            return deferred.promise;
        }

        function _getStudents() {
            var deferred = $q.defer();

            $http.get(sharedValues.apiUrl + 'users/getStudents').then(function (res) {

                var users = res.data;
                deferred.resolve(users);

            });


            return deferred.promise;
        }

        function _getUser(id, isForCartis) {
           
            if (!isForCartis) isForCartis = false;

           
            var deferred = $q.defer();
            if (id == 0) {
                $http.get(sharedValues.apiUrl + 'users/newuser/').then(function (res) {
                    res.data.Meta = {};
                    deferred.resolve(res.data);
                });
            }
            else {

                if (isForCartis) {
                    $http.get(sharedValues.apiUrl + 'users/getsetUserEnter/' + isForCartis + '/' + (id || '')).then(function (res) {
                        var user = res.data;
                        deferred.resolve(user);
                    });


                } else {
                    $http.get(sharedValues.apiUrl + 'users/getUser/' + (id || '')).then(function (res) {
                        var user = res.data;
                        deferred.resolve(user);
                    });


                }


              
            }
            return deferred.promise;
        }

        function _getPaymentsByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getpaymentsbyuserid/' + (id || '')).then(function (res) {
              
                var user = res.data;
                
                deferred.resolve(user);
            });
            return deferred.promise;
        }




        function _getAvailablehours(id) {

          
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getAvailablehours/' + (id || '')).then(function (res) {
                var user = res.data;

                deferred.resolve(user);
            });
            return deferred.promise;
        }

        

        function _getUserUserhorsesByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getuseruserhorsesbyuserid/' + (id || '')).then(function (res) {
                var user = res.data;
                 
                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _getAllFarmsuseruserhorses() {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getAllFarmsuseruserhorses').then(function (res) {
                var horses = res.data;

                deferred.resolve(horses);
            });
            return deferred.promise;
        }





        function _getUserFilesByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getuserfilesbyuserid/' + (id || '')).then(function (res) {
                var user = res.data;
              
                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _getUserCommitmentsByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getusercommitmentsbyuserid/' + (id || '')).then(function (res) {
                var user = res.data;
             
                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _getUserExpensesByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getuserexpensesbyuserid/' + (id || '')).then(function (res) {
                var user = res.data;
              
                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _getUserUserMakavByUserId(id) {
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getuserusermakavbyuserid/' + (id || '')).then(function (res) {
                var user = res.data;

                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _updateUser(user) {
           
            var deferred = $q.defer();
         //   user.Meta = angular.toJson(user.Meta);
           
            $http.post(sharedValues.apiUrl + 'users/updateuser', user).then(function (res) {
             
                var user = res.data;
              //  user.Meta = angular.fromJson(user.Meta);
                deferred.resolve(user);
            });
            return deferred.promise;
        }

        function _updateUserMultiTables(user, payments, files, commitments, expenses, userhorses, availablehours,makav,checks,ashrais) {
          
          
            var dataobj = [user, payments, files, commitments, expenses, userhorses, availablehours, makav, checks, ashrais];
            var deferred = $q.defer();
           // user.Meta = "";//angular.toJson(user.Meta);

          

            $http.post(sharedValues.apiUrl + 'users/updateusermultitables', angular.toJson(dataobj)).then(function (res) {

                var user = res.data;
             //   user.Meta = angular.fromJson(user.Meta);
                deferred.resolve(user);
            });
            return deferred.promise;

        }

        //function _importUsers(users) {
        //    
        //    for (var user of users) {
        //        user.Meta = angular.toJson(user.Meta);
        //    }
        //    $http.post(sharedValues.apiUrl + 'users/importusers', users).then(function () {
        //        deferred.resolve();
        //    });
        //    return deferred.promise;
        //}
        function _importUsers(users) {
            var deferred = $q.defer();
            $http.post(sharedValues.apiUrl + 'users/importusers', users).then(function () {
                deferred.resolve();
            });
            return deferred.promise;
        }


        function _deleteUser(id) {
            var deferred = $q.defer();
           

            $http.get(sharedValues.apiUrl + 'users/deleteUser/' + id).then(function (res) {
                deferred.resolve();
            });
            return deferred.promise;
        }

        function _getUserIdByEmail(email) {
            return $http.get(sharedValues.apiUrl + 'users/getUserIdByEmail/' + email + '/');
        }

        function _roles() {
            return sharedValues.roles;
        }

        function _report(type, fromDate, toDate) {
         
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getReport/' + type + "/" + fromDate + "/" + toDate).then(function (res) {
                var data = res.data;

                deferred.resolve(data);
            });
            return deferred.promise;
        }
        function _reportHMO(fromDate, toDate) {
         
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getReportHMO/' + fromDate + "/" + toDate).then(function (res) {
                var data = res.data;

                deferred.resolve(data);
            });
            return deferred.promise;
        }

        function _reportMaccabi(fromDate, toDate) {

            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getReportMaccabi/' + fromDate + "/" + toDate).then(function (res) {
                var data = res.data;

                deferred.resolve(data);
            });
            return deferred.promise;
        }

        function _reportKlalit(fromDate, toDate) {

            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getReportKlalit/' + fromDate + "/" + toDate).then(function (res) {
                var data = res.data;

                deferred.resolve(data);
            });
            return deferred.promise;
        }

        function _reportDebt() {

            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getReportDebt').then(function (res) {
                var data = res.data;

                deferred.resolve(data);
            });
            return deferred.promise;
        }

        
        function _getTransferData(insructorId, dow,date) {
       
            var deferred = $q.defer();
            $http.get(sharedValues.apiUrl + 'users/getTransferData/' + insructorId + "/" + dow + "/" + date).then(function (res) {
                var data = res.data;
              
                deferred.resolve(data);
            });
            return deferred.promise;
        }








    }

})();