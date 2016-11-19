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
            dataType: 'json',
            statusCode: {
                200: function () {
                    window.location.href = '/Dashboard/';
                },
                202: function () {
                    window.location.href = '/Dashboard/';
                },
                403: function () {
                    $(".login-fail").show();
                }
            }
        });
    }
};

ko.applyBindings(landingPageViewModel);