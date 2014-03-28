
$(document).ready(function () {
    var post = $.connection.postHub;
    $.connection.hub.start();

    post.client.refreshPost = function (title) {
        toastr.success(title, "Feed Processed");
    };
});

