var createCalendarViewModel = {
    name: ko.observable(''),
    create: function () {
        $.ajax(
        {
            url: '/API/Calendar/CreateCalendar',
            type: 'POST',
            data: {
                name: this.name()
            },
            dataType: 'json'
        }).done(function () {
            alert('created');
        });
    }
};

ko.applyBindings(createCalendarViewModel, document.getElementById('page-wrapper'));