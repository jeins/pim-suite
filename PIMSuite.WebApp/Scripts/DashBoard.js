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
            // todo
        });
    }
};

ko.applyBindings(dashboardViewModel, document.getElementsByClassName('navbar')[0]);