
$(document).ready(function () {

    var bindData = function(data) {

        var viewModel = {
            groups: ko.observableArray(data)
        };
        ko.applyBindings(viewModel);
    };

    $.ajax({
        url: "/Group/GetGroups",
        success: function (result) {
            bindData(result);
            $("#groupTable").show();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
});