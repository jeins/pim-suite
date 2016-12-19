var createCalendarViewModel = {
    name: ko.observable(''),
    creationSuccessful: ko.observable(false),
    creationFailed: ko.observable(false),
    create: function () {
        this.creationSuccessful(false);
        this.creationFailed(false);
        $.ajax(
        {
            url: '/API/Calendar/CreateCalendar',
            type: 'POST',
            data: {
                name: this.name()
            },
            dataType: 'json'
        }).done(function () {
            createCalendarViewModel.creationSuccessful(true);
            createCalendarViewModel.name('');
        }).fail(function() {
            createCalendarViewModel.creationFailed(true);
        });
    }
};

ko.applyBindings(createCalendarViewModel, document.getElementById('page-wrapper'));