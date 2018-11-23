$(document).ready(function () {
    $(".createNewDialog").click(function (event) {
        event.preventDefault();
        var color = $(this).data('color');
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id-value');
        var name = $buttonClicked.attr('data-name');
        var options = { keyboard: true, focus: true };
        $.ajax({
            type: "GET",
            url: insertUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);

                $('#mdModal').modal('show');

                $('#mdModal').on('shown.bs.modal', function () {
                    $('#mdModal').find('.modal-body').html(response);
                    // $('#mdModal').find('.modal-title').html("Operation details");
                });
            },
            failure: function (response) {

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);

                $('#mdModal').modal('show');

                $('#mdModal').on('shown.bs.modal', function () {
                    $('#mdModal').find('.modal-body').html("Problem loading your data...");
                });
            },
            error: function (response) {

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);

                $('#mdModal').modal('show');

                $('#mdModal').on('shown.bs.modal', function () {
                    $('#mdModal').find('.modal-body').html("Problem loading your data...");
                });
            }
        });
    });
});