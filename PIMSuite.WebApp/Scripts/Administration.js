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

ko.applyBindings(domainViewModel, document.getElementById("domainBlock"));