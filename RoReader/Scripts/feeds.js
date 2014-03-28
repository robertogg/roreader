
$(document).ready(function () {

    var bindData = function(data) {

        var viewModel = {
            feeds: ko.observableArray(data)
        };
        ko.applyBindings(viewModel);
    };

    $.ajax({
        url: "/Feed/GetFeeds",
        success: function (result) {
            bindData(result);
            $("#feedTable").show();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
});