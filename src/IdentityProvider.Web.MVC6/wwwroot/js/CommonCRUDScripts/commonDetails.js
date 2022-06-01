// Item details modal
$(document).ready(function () {
    $(".detailsDialog").click(function (event) {
        event.preventDefault();

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr("data-id-value");

        var options = { 'backdrop': 'static', keyboard: true, focus: true };
        $.ajax({
            type: "POST",
            url: detailUrl,
            data: '{id: "' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#mdModal").modal(options);

                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html(response);

                $("#mdModal").on("shown.bs.modal",
                    function () {
                    });
            },
            failure: function (response) {
                $("#mdModal").modal(options);

                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html("Problem loading your data...");

                $("#mdModal").on("shown.bs.modal",
                    function () {
                    });
            },
            error: function (response) {
                $("#mdModal").modal(options);

                $("#mdModal").find(".modal-body").html("");
                $("#mdModal").modal("show");
                $("#mdModal").find(".modal-body").html("Problem loading your data...");

                $("#mdModal").on("shown.bs.modal",
                    function () {
                    });
            }
        });
    });
});