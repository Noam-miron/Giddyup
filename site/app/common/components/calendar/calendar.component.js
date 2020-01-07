(function ($) {

    var app = angular.module('app');

    app.component('calendar', {
        template: '<div class="calendar"></div>',
        controller: CalendarController,
        bindings: {
            events: '<',
            backgroundEvents: '<',
            resources: '<',
            eventChangeCallback: '< eventChange',
            eventClickCallback: '< eventClick',
            eventCreateCallback: '< eventCreate',
            pastEditEnabled: '< pastEdit',
            view: '@'
        }
    });

    function CalendarController($element, $scope, $rootScope) {



        this.viewRender = _viewRender.bind(this);
        this.eventResize = _eventResize.bind(this);
        this.eventDrop = _eventDrop.bind(this);
        this.eventClick = _eventClick.bind(this);
        this.eventSelect = _eventSelect.bind(this);
        this.reloadEvents = _reloadEvents.bind(this);
        this.reloadResources = _reloadResources.bind(this);
        this.loadResources = _loadResources.bind(this);
        this.loadEvents = _loadEvents.bind(this);


        this.pastEditEnabled = this.pastEditEnabled || true;
        this.calendar = $($element).children('.calendar')[0];
        this.scope = $scope;
        this.currentView = "";

        this.scope.$on('calendar.reloadEvents', function (event, events) {

            this.events = events;
            this.reloadEvents();





        }.bind(this));

        this.scope.$on('calendar.reloadBackgroundEvents', function (event, backgroundEvents) {
            this.backgroundEvents = backgroundEvents;
            this.reloadEvents();
            //  alert(12);
        }.bind(this));

        this.scope.$on('calendar.reloadResources', function (event, resources) {
            this.resources = resources;
            this.reloadResources();
        }.bind(this));

        $(window).scroll(function (e) {

            $(this).off("scroll");
            // event.stopPropagation();
            //   return false;
            //if ($(this).scrollTop() == 0) {
            //    alert();
            //}
        });

        $('.calendar').fullCalendar('destroy');
        $(this.calendar).fullCalendar({
            header: {
                right: 'today next,prev',
                center: 'title',
                left: 'month,agendaWeek,agendaDay'
            },
            buttonText: {
                month: 'חודש',
                agendaWeek: 'שבוע',
                agendaDay: 'יום'
            },

            titleFormat: 'DD/MM/YYYY ddd',
            weekends: true,
            hiddenDays: [7],
            schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
            groupByResource: false,
            groupByDateAndResource: true,

            lang: 'he',
            isRTL: true,

            defaultView: this.view || 'agendaWeek',





            selectHelper: true,
            editable: true,
            slotEventOverlap: true,
            slotDuration: '00:15:00',
            slotLabelInterval: '00:15:00',
            slotLabelFormat: 'HH:mm',
            allDaySlot: false,

            height: 600,//($rootScope.isPhone)?450:600,//600

            minTime: '08:00',
            maxTime: '24:00',

            events: this.loadEvents.bind(this),
            resourceOrder: 'id',
            resources: this.loadResources.bind(this),

            eventClick: this.eventClick.bind(this),
            select: this.eventSelect.bind(this),

            eventDrop: this.eventDrop.bind(this),
            viewRender: this.viewRender.bind(this),

            //************* בשביל מדריכים ללא הרשאה
            //eventStartEditable: false,
            //eventResize: false,
            //selectable: false,
            //eventDurationEditable:false,

            eventStartEditable: true,
            eventResize: this.eventResize.bind(this),
            selectable: true,

            //******************************
            eventResizeStart: function (event, element) {

                if (($rootScope.isPhone)) {

                    $('html, body,.fc-time-grid-container').css({
                        overflow: 'hidden',
                        height: 'auto'
                    });
                }



            }.bind(this),

            eventDragStart: function (event, element) {
                //   alert();
                if (($rootScope.isPhone)) {

                    $('html, body,.fc-time-grid-container').css({
                        overflow: 'hidden'

                    });
                }



            }.bind(this),

            eventDragStop: function (event, element) {

                $('html, body.fc-time-grid-container').css({
                    overflow: 'auto',
                    height: 'auto'
                });


            }.bind(this),

            eventResizeStop: function (event, element) {

                $('html, body').css({
                    overflow: 'auto',
                    height: 'auto'
                });


            }.bind(this),



            eventRender: function (event, element) {

                var IsFuture = true;//(event.end) ? (event.end.format('YYYYMMDD') <= moment().format('YYYYMMDD')) : false;

                if (event.rendering != "background" && event.statuses && event.statuses.length > 0) {


                    for (var i in event.statuses) {

                        if (IsFuture && !event.statuses[i].Status || event.statuses[i].Status == '') {

                            $(element).addClass('warning-icon');
                        }

                        else if (event.statuses[i].Status == 'notAttended') {

                            $(element).addClass('notarrive-icon');
                        }



                            //לא הגיע לא לחייב
                        else if (IsFuture && event.statuses[i].Status == 'notAttendedDontCharge') {

                            $(element).addClass('redvi2-icon');
                        }

                            //לא הגיע  לחייב
                        else if (IsFuture && event.statuses[i].Status == 'notAttendedCharge') {

                            $(element).addClass('redvi-icon');
                        }

                        else if (event.statuses[i].Status == 'completion' && (event.statuses[i].IsComplete == 3 || event.statuses[i].IsComplete == 5)) {
                            $(element).addClass('returnred-iconfloat');
                        }

                        else if (event.statuses[i].Status == 'completion' && (event.statuses[i].IsComplete == 4 || event.statuses[i].IsComplete == 6)) {
                            $(element).addClass('returngreen-iconfloat');
                        }

                            //  במידה ויש דרוש שיעור השלמה על ההשלמה
                            // כנראה מיותר
                        else if (event.statuses[i].Status == 'completionReq' && event.statuses[i].IsComplete == 5) {

                            //   $(element).css("color", "white").css("background", "Silver").css("border-color", "gray");
                            $(element).addClass('returngreen-icon');
                        }

                            // הוגדר שהוא צריך שיעור השלמה
                        else if (event.statuses[i].Status == 'completionReq' && event.statuses[i].IsComplete == 1) {

                            $(element).css("background-color", "lightGray").css("border-color", "gray");
                            $(element).addClass('returnred-icon');
                            $(element).addClass("hadPeami");
                        }

                            // שמו לו שיעור השלמה איפה שהוא
                        else if (event.statuses[i].Status == 'completionReq' && event.statuses[i].IsComplete == 2) {

                            $(element).css("background-color", "lightGray").css("border-color", "gray");

                            $(element).addClass('returngreen-icon');
                            $(element).addClass("hadPeami");
                        }

                        else if (IsFuture) {

                            $(element).addClass('approve-icon');
                        }


                    }


                    //if (!$(element).hasClass('warning-icon')) {
                    //$(element).addClass('approve-icon');
                    //}




                }

                if (event.title == "ניתן להכניס חד פעמי") {
                    $(element).css("color", "white").css("background", "gray").css("border-color", "Silver");
                    $(element).addClass("hadPeami");
                } else if (event.horsenames && event.horsenames.length > 0) {

                    var horsenames = "";
                    var prefix = "";
                    for (var i = 0; i < event.horsenames.length; i++) {
                        if (i != 0) prefix = ",";
                        horsenames = horsenames + prefix + event.horsenames[i];
                    }


                    $(element).find(".fc-time span").css("float", "right").before("<span class='spHorse'> (" + horsenames + ")  </span>");
                }



            }.bind(this)

        });

        $(".addedCalender").prependTo(".fc-right");


        //Scroll To End 



    }





    function _viewRender(view, element) {


        this.scope.$emit('calendar.viewRender', { startDate: view.start, endDate: view.end });
        this.reloadEvents();





    }

    function _eventResize(event) {


        this.eventChangeCallback(event);
        this.reloadEvents();
        this.scope.$apply();
    }

    function _eventDrop(event) {



        this.eventChangeCallback(event);
        this.reloadEvents();
        this.scope.$apply();


    }

    function _eventClick(event, jsEvent) {

        //if (!jsEvent.target.id) {
        this.eventClickCallback(event, jsEvent);
        this.reloadEvents();
        this.scope.$apply();


        //}
    }

    function _eventSelect(start, end, jsEvent, view, resource) {

        if (!this.pastEditEnabled && moment(start).isBefore(new Date())) {
            alert('תחילת שיעור לא יכולה להיות בעבר');
        }
        else {
            this.eventCreateCallback(start, end, jsEvent, view, resource);
        }
        $(this.calendar).fullCalendar('unselect');
        this.reloadEvents();
        this.scope.$apply();
    }

    function _reloadEvents() {



        $(this.calendar).fullCalendar('refetchEvents');

        $(".fc-title").each(function () {

            var currentElement = $(this).text();

            //alert(currentElement);
            $(this).html(currentElement);

        });





    }

    function _reloadResources() {
        $(this.calendar).fullCalendar('refetchResources');
        $(".fc-title").each(function () {
            var currentElement = $(this).text();

            $(this).html(currentElement);

        });



        var view = $('.calendar').fullCalendar('getView');

       

        var resCount = this.resources.length;
      
        if (resCount > 6) {

            var setWidth = resCount * 16.66666666;

            $(".fc-view.fc-agendaDay-view.fc-agenda-view").css("width", setWidth + "%");
            $(".fc-view.fc-agendaWeek-view.fc-agenda-view").css("width", setWidth + "%");
            $(".fc-view.fc-month-view.fc-basic-view").css("width", setWidth + "%");
           
            if (view.type != this.currentView) {
               // alert(22);
                $(".fc-view-container").scrollLeft(5000);
                this.currentView = view.type;

            }
          
        } else {
            $(".fc-view.fc-agendaDay-view.fc-agenda-view").css("width", "100%");
            $(".fc-view.fc-agendaWeek-view.fc-agenda-view").css("width", "100%");
            $(".fc-view.fc-month-view.fc-basic-view").css("width", "100%");


        }


       

      


    }

    function _loadResources(callback) {
        callback(this.resources);

    }

    function _loadEvents(start, end, timezone, callback) {

        callback(this.events.concat(this.backgroundEvents || []));

    }

})(jQuery);