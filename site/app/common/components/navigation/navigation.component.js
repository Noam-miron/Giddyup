﻿(function () {
    var app = angular.module('app');

    app.directive('navigation', function () {
        return {
            templateUrl: 'app/common/components/navigation/navigation.template.html',
            controller: NavigationController,
            controllerAs: '$ctrl',
            replace: true,
        }
    });

    function NavigationController($scope, $rootScope) {
        this.init = _init.bind(this);


        $rootScope.$on('$stateChangeSuccess', this.init);

        function _init() {

            if (location.href.indexOf("userp") != -1) {

                this.farms =  false;
                this.accounting = false;
                this.lessons =  false;
                this.users = false;
                this.instructors =  false;
                this.students = false;
                this.reports =  false;
                this.horses =  false;

                this.farmmanager =  false;
               // this.files = role != null;
                this.files = null;
                this.username = null;
                

                return;
            }
            var role = localStorage.getItem('currentRole');
            this.role = role;

            this.subrole = localStorage.getItem('currentSubRole');

           
           
            this.farms = ['sysAdmin'].indexOf(role) != -1 ? true : false;
            this.accounting = ['sysAdmin', 'farmAdmin', 'farmAdminHorse'].indexOf(role) != -1 ? true : false;
            this.lessons = ['sysAdmin', 'farmAdmin', 'instructor', 'profAdmin', 'farmAdminHorse', "stableman", "worker", 'vetrinar', 'shoeing'].indexOf(role) != -1 ? true : false;
            this.users = ['sysAdmin', 'farmAdmin'].indexOf(role) != -1 ? true : false;
            this.instructors = (['sysAdmin', 'farmAdmin', 'farmAdminHorse', 'instructor', 'profAdmin', 'vetrinar', 'shoeing'].indexOf(role) != -1 && ["stableman", "worker"].indexOf(this.subrole) == -1 )? true : false;
            this.students = ['sysAdmin', 'farmAdmin', 'farmAdminHorse', 'vetrinar', 'shoeing'].indexOf(role) != -1 ? true : false;
            this.reports = ['sysAdmin', 'farmAdmin', 'farmAdminHorse', 'vetrinar', 'shoeing'].indexOf(role) != -1 ? true : false;
            this.horses = ['sysAdmin', 'farmAdmin', 'profAdmin', 'stableman', "worker", 'assistant', 'farmAdminHorse', 'vetrinar', 'shoeing'].indexOf(role) != -1 || ["stableman", "worker"].indexOf(this.subrole) != -1 ? true : false;

            this.farmmanager = ['sysAdmin', 'farmAdmin', 'farmAdminHorse'].indexOf(role) != -1 ? true : false;
            this.files = role != null;

            var authData = localStorage.getItem('authorizationData');
            if (authData) {
                authData = angular.fromJson(authData);
                this.username = authData.userName;
            }
            else {
                this.username = null;
            }
        }
    }

})();