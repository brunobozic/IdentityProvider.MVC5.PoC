// Submit the form on changing the dropbox selected item value (paging: items per page)
$(document).ready(function () {
    $("#PageSizeList").select2({
        // theme: "classic"
    });

    $('#PageSizeList').on('select2:select', function (event) {
        var form = $(event.target).closest("form");
        form.submit();
    });
});