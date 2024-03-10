$(document).ready(function () {
    $(".OperationsDashboard_OperationsDatatable_create").click(function (event) {
        event.preventDefault();
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
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html(response);

                $('#mdModal').on('shown.bs.modal', function () {
                });

                skinChanger();
            },
            failure: function (response) {

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').find('.modal-body').html("Problem loading your data...");

                $('#mdModal').on('shown.bs.modal', function () {
                
                });

                skinChanger();
            },
            error: function (response) {

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').find('.modal-body').html("Problem loading your data...");

                $('#mdModal').on('shown.bs.modal', function () {

                });

                skinChanger();
            }
        });
    });
});