var showCalendarViewModel = {
    eventId: ko.observable(),
    startDate: ko.observable(),
    endDate: ko.observable(),
    title: ko.observable(),
    location: ko.observable(),
    description: ko.observable(),
    isPrivate: ko.observable(false),
    isConfirmed: ko.observable(false),
    editEvent: function() {
        $.ajax(
        {
            url: '/API/CalendarEvent/EditEvent',
            type: 'POST',
            data: {
                eventId: showCalendarViewModel.eventId(),
                start: showCalendarViewModel.startDate(),
                end: showCalendarViewModel.endDate(),
                title: showCalendarViewModel.title(),
                location: showCalendarViewModel.location(),
                description: showCalendarViewModel.description(),
                isPrivate: showCalendarViewModel.isPrivate(),
                isConfirmed: showCalendarViewModel.isConfirmed()
            },
            dataType: 'json'
        }).done(function () {
            location.reload();
        });
    },
    deleteEvent: function () {
        $.ajax(
        {
            url: '/API/CalendarEvent/DeleteEvent?eventId=' + showCalendarViewModel.eventId(),
            type: 'POST',
            data: {
                eventId: showCalendarViewModel.eventId()
            },
            dataType: 'json'
        }).done(function () {
            location.reload();
        });
    }
};

$(function() {
    ko.applyBindings(showCalendarViewModel, document.getElementsByClassName('view-event-dialog')[0]);
});

$(function(){
    $('#eventForm').validator();
});

var calendar = $('#calendar')
    .fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek'
        },
        selectable: true,
        selectHelper: true,
        select: function (start) {
            $('#add_event').click();

            $('#add_start').val(moment(start).format('DD-MM-YYYY HH:mm'));
            $('#add_title').val("");
            $('#add_end').val("");
            $('#add_location').val("");
            $('#add_description').val("");
            $('#add_is_private').prop('checked', false);

            $(".add_submit").on('click', function() {  
                if ($('#add_title').val().length!=0 &&  $('#add_location').val().length!=0 && $('#add_description').val()!=0 && $('#add_start').val().length!=0 && $('#add_end').val().length!=0 && $('#add_start').val() < $('#add_end').val()){
                    var newEvent = {
                        title: $('#add_title').val(),
                        start: moment($('#add_start').val(), 'DD-MM-YYYY HH:mm').format("LLLL"),
                        end: moment($('#add_end').val(), 'DD-MM-YYYY HH:mm').format("LLLL"),
                        location: $('#add_location').val(),
                        description: $('#add_description').val(),
                        allDay: false,
                        calendarId: calendarId,
                        isPrivate: $('#add_is_private').prop('checked')
                    };

                    calendar.fullCalendar('renderEvent', newEvent, true);
                    calendar.fullCalendar('unselect');

                    $.ajax({
                        type: 'POST',
                        url: '/API/CalendarEvent/CreateNewEvent',
                        data: newEvent
                    });

                    $('.add_close').click();

                    return false;
                }

                else{
                    if ($('#add_start').val() > $('#add_end').val())
                    {
                        alert("Der Zeitraum für den Termin wurde nicht korrekt eingegeben! Versuchen Sie noch mal.");
                    }
                    return true;
                }
            });

        },
        eventClick: function(calEvent) {
            $('#view_event').click();
            showCalendarViewModel.eventId(calEvent.id);
            showCalendarViewModel.startDate(moment(calEvent.start).format('DD-MM-YYYY HH:mm'));
            showCalendarViewModel.endDate(moment(calEvent.end).format('DD-MM-YYYY HH:mm'));
            showCalendarViewModel.title(calEvent.title);
            showCalendarViewModel.location(calEvent.location);
            showCalendarViewModel.description(calEvent.description);
            showCalendarViewModel.isPrivate(calEvent.isPrivateEvent);
            showCalendarViewModel.isConfirmed(calEvent.isConfirmed);

            calendar.fullCalendar('unselect');
        },
        events: ('/API/CalendarEvent/GetEvents?userId=' + userId + '&calendarId=' + calendarId)
    });

$(function () {
    $('#add_start').datetimepicker({ format: 'DD-MM-YYYY HH:mm' });
    $('#add_end').datetimepicker({ format: 'DD-MM-YYYY HH:mm' });
    $('#view_start').datetimepicker({ format: 'DD-MM-YYYY HH:mm' });
    $('#view_end').datetimepicker({ format: 'DD-MM-YYYY HH:mm' });
});

$(function () {
    $("#subscribe").click(function () {
        var self = $(this);
        if (self.val() == "abonnieren") {
            $.ajax({
                type: "POST",
                url: "/Calendar/CreateSubscription",
                data: '{calendarId: "' + calendarId + '" }',
                contentType: "application/json; charset=utf-8",
                    
            });
            self.val("deabonnieren");
            $.notify("Jetzt abonnieren Sie den Kalender", "success");
        }
        else {
            if (self.val() == "deabonnieren") {
                $.ajax({
                    type: "POST",
                    url: "/Calendar/RemoveSubscription",
                    data: '{calendarId: "' + calendarId+ '" }',
                    contentType: "application/json; charset=utf-8",
                        
                });
                
                self.val("abonnieren");
                $.notify("Sie abonnieren den Kalender nicht mehr", "warn");
            }
        }
    });
});