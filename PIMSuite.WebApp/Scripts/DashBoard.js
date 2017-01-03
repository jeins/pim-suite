var dashboardViewModel = {

    logout: function () {
        $.ajax(
        {
            url: '/API/Logout/Logout',
            type: 'POST',
            data: {
            },
            dataType: 'json'
        }).done(function () {
            window.location.href = '/';
        });
    }
};

ko.applyBindings(dashboardViewModel, document.getElementsByClassName('navbar')[0]);