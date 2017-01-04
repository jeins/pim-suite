var dashboardViewModel = {
    hasNotifications: ko.observable(hasNotifications),
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
    },
    clearNotifications: function() {
        $.ajax(
        {
            url: '/API/Notification/ClearNotifications',
            type: 'POST',
            data: {
            },
            dataType: 'json'
        }).done(function () {
            dashboardViewModel.hasNotifications(false);
        });
    }
};

ko.applyBindings(dashboardViewModel, document.getElementsByClassName('navbar')[0]);