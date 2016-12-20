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

var domainViewModel = {
    newDomain: ko.observable(),
    addDomain: function () {
        $.ajax(
        {
            url: '/API/Domain/Add',
            type: 'POST',
            data: {
                NewDomain: this.newDomain(),
            },
            dataType: 'json',
            statusCode: {
                200: function () {
                    window.location.href = '/Dashboard/';
                }
            }
        });
    }
}

ko.applyBindings(dashboardViewModel, document.getElementsByClassName('navbar')[0]);
ko.applyBindings(domainViewModel, document.getElementById("domainBlock"));