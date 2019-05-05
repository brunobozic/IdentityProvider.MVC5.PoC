$(function () {

    $("#frmEditItem").on("submit", function (event) {
        var $this = $(this);

        event.preventDefault();

        $("#loading").show();
        $('#btnSubmit').attr('disabled', 'disabled');

        var dataToPost = $(this).serialize({ checkboxesAsBools: true });
        alert(dataToPost);
        $.ajax({
            type: $this.attr('method'),
            url: $this.attr('action'),
            data: dataToPost,
            success: function (response) {

                if (response.Success === true) {
                    $('#mdModal').modal('toggle');
                    swal(
                        'Edits saved',
                        'Your item has been editted.',
                        'success'
                    ).then(function () {
                        location.reload();
                    });
                }
                else {

                    if (response.OptimisticConcurrencyError === true) {
                        swal(
                            'Warning!',
                            'Your item has not been editted. ' + response.OptimisticConcurrencyErrorMsg,
                            'error'
                        ).then(function () {
                            location.reload();
                        });
                    } else {
                        swal(
                            'Edits failed',
                            'Your item has not been editted.',
                            'error'
                        ).then(function () {
                            for (var i = 0; i < response.FormErrors.length; i++) {
                                $('span[data-valmsg-for="' + response.FormErrors[i].key + '"]').text(response.FormErrors[i].errors[0]);
                            }
                            // now apply the style name of class eq to "error" to all the elements with the class "field-validation-valid"
                            $(function () {
                                $('.field-validation-valid').each(function () {
                                    $(this).addClass('error');
                                });
                            });
                        });
                    }
                }

                return false;
            },
            error: function (response) {
                swal(
                    'Error!',
                    'Your item has not been editted. ' + response.ValidationIssues,
                    'error'
                ).then(function () {
                    location.reload();
                });
            },
            complete: function () {
                $("#loading").hide();
                $('#btnSubmit').removeAttr('disabled');
            }
        });
    });
});