var landingPageViewModel = {
    emailOrUser: ko.observable(),
    password: ko.observable(),

    login: function() {
        $.ajax(
        {
            url: '/API/Login/Login',
            type: 'POST',
            data: {
                EmailOrUser: this.emailOrUser(),
                Password: this.password()
            },
            dataType: 'json'
        }).done(function() {
            window.location.href = '/Dashboard/Dashboard';
        });
    }
};

ko.applyBindings(landingPageViewModel);