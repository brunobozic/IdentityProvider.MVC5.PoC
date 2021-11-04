$(document).ready(function() {

    $("#ShowInactive").change(function() {

        if (this.checked) {
            $(this).prop("checked", true);
            $("#ShowInactive").val(this.checked);
            $("#ShowInactive").prop("checked", true);
            $("#ShowInactive").attr("checked", true);
            $("#ShowInactiveHidden").val("true");
        } else {
            $(this).prop("checked", false);
            $("#ShowInactive").prop("checked", "");
            $("#ShowInactive").attr("checked", "");
            $("#ShowInactiveHidden").val("false");
            $("#ShowInactive").removeAttr("checked");
        }
    });

    $("#ShowDeleted").change(function() {

        if (this.checked) {
            $(this).prop("checked", true);
            $("#ShowDeleted").prop("checked", true);
            $("#ShowDeleted").attr("checked", true);
            $("#ShowDeletedHidden").val("true");
        } else {
            $(this).prop("checked", false);
            $("#ShowDeleted").prop("checked", "");
            $("#ShowDeleted").attr("checked", "");
            $("#ShowDeletedHidden").val("false");
            $("#ShowDeleted").removeAttr("checked");
        }
    });
});