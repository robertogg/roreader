
$(document).ready(function () {
    var bindData = function (data) {
        var viewModel = {
            posts: ko.observable(data),
            onShowPost: function (dataRow) {
                var postId = dataRow.RowKey;
                $.ajax({
                    url: "/Post/GetPost/" + postId,
                    success: function (result) {
                        $("#postTitel").html(result.Title);
                        $(".contentPannel").html(result.Content);
                        $("html").scrollTo(0);
                    }
                });
            }
        };
        ko.applyBindings(viewModel);
    };

    $.ajax({
        url: "/Post/GetPosts",
        success: function (result) {
            bindData(result);
            $("#posts").show();
        }
    });

    $(".menuPannel").delegate(".itemGroup", "click", function () {
        $(this).next(".itemFeeds").toggle();
    });

    $(".menuPannel").delegate(".itemFeed", "click", function () {
        $(this).next(".itemPosts").toggle();
    });
});