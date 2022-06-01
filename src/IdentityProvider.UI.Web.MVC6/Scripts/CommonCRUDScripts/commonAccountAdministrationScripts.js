// Item edit modal
$(document).ready(function () {
    $(".administerAccountDialog").click(function (event) {
        event.preventDefault();
        var options = { 'backdrop': 'static', keyboard: true, focus: true };

        $.ajax({
            type: "GET",
            url: accountAdministrationUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#mdModal").modal(options);
                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html(response);
            },
            failure: function (response) {
                $("#mdModal").modal(options);
                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html("Problem loading your data...");
            },
            error: function (response) {
                $("#mdModal").modal(options);
                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html("Problem loading your data...");
            }
        });
    });
});