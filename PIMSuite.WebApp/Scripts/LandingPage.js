var landingPageViewModel = {
    emailAddress: ko.observable(),
    password: ko.observable(),

    login: function() {
        $.ajax(
        {
            url: '/API/Login/Login',
            type: 'POST',
            data: {
                EmailAddress: this.emailAddress(),
                Password: this.password()
            },
            dataType: 'json'
        }).done(function() {
            window.location.href = '/Portal/';
        });
    }
};

ko.applyBindings(landingPageViewModel);