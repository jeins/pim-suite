var createCalendarViewModel = {
    name: ko.observable(''),
    isPrivate: ko.observable(false),
    create: function () {
        $.ajax(
        {
            url: '/API/Calendar/CreateCalendar',
            type: 'POST',
            data: {
                name: this.name(),
                isPrivate: this.isPrivate()
            },
            dataType: 'json'
        }).done(function () {
            alert('created');
        });
    }
};

ko.applyBindings(createCalendarViewModel, document.getElementById('page-wrapper'));