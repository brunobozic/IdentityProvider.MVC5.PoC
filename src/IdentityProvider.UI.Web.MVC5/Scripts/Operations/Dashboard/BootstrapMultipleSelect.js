// ======== Bootstrap multiselect ========

$(document).ready(function () {
    $('#OperationsDashboard_AuditTrailDatatable_TableName_MS').select2({
        width: '100%', // need to override the changed default
        placeholder: 'Select table name',
        dropdownAutoWidth: true,
        // dropdownParent: $('.multiselect-parent')
    });

    $('#OperationsDashboard_AuditTrailDatatable_Actions_MS').select2({
        width: '100%', // need to override the changed default
        placeholder: 'Select an action',
        dropdownAutoWidth: true,
        // dropdownParent: $('.multiselect-parent')
    });

    //  I first hide the default tag that's being used for the down arrow
    $('b[role="presentation"]').hide();
    // Then with CSS I style the select boxes. I set the height, change the background color of the arrow area to a gradient black, change the width, font-size and also the color of the down arrow to white
    $('.select2-selection__arrow').append('<i class="fa fa-angle-down"></i>');
});

// ======== #ENDOF# Bootstrap multiselect ========