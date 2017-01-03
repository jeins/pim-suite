var deleteEventViewModel = {
    deleteevent: function () {
        eventid = document.getElementById('view_event_id').value;
        $.ajax(
        {
            url: '/API/CalendarEvent/DeleteEvent?eventId='+eventid,
            type: 'POST',
            data: {
                eventId: eventid
            },
            dataType: 'json'
        }).done(function () {
            location.reload();
        });
    }
};

ko.applyBindings(deleteEventViewModel, document.getElementsByClassName('deletebutton')[0]);