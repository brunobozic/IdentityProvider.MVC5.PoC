// Item edit modal
$(document).ready(function () {
    $(".editDialog").click(function (event) {
        event.preventDefault();
        var color = $(this).data('color');

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id-value');

        var name = $buttonClicked.attr('data-name');
        var options = { /*'backdrop': 'static',*/ keyboard: true, focus: true };
        $.ajax({
            type: "GET",
            url: editUrl + '?id=' + id,
            //data: '{id: "' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {

                $('#mdModal').find('.modal-body').html("");

                // $('#mdModal .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);

                $('#mdModal').modal(options);

                $('#mdModal').modal('show');

                $('#mdModal').find('.modal-body').html(response);

                try {
                    var $buttons = $(":button");
                    var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
                    $buttons.removeClass('themed-buttons-' + existTheme);
                    $buttons.removeClass('bg-' + existTheme);
                    var that = $('.right-sidebar .demo-choose-skin li');
                    $buttons.addClass('themed-buttons-' + existTheme);
                    $buttons.addClass('bg-' + existTheme);
                } catch (exception) {
                    console.log("operationEditPartial editDialog onClick problem", exception);
                }

                // $('#mdModal').modal('show');

                //$('#mdModal').on('shown.bs.modal', function () {

                //});
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