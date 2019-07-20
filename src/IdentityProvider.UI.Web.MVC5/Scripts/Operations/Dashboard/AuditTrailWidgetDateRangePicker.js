var dateFormat = 'DD.MM.YYYY HH:MM:SS';

$(function () {
    // Re-draw the table when the a date range filter changes
    $('input[name="OperationsDashboard_AuditTrailDatatable_DateRange"]').on('apply.daterangepicker', function (ev, picker) {
        console.log(picker.startDate.format(dateFormat));
        console.log(picker.endDate.format(dateFormat));
        table.draw();
    });

    $('input[name="OperationsDashboard_AuditTrailDatatable_DateRange"]').daterangepicker({
        timePicker: true,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(24, 'hour'),
        autoUpdateInput: false,
        locale: {
            format: dateFormat,
            cancelLabel: 'Clear'
        },
        "minYear": 2000,
        "maxYear": 2050,
        "timePicker24Hour": true,
        "timePickerSeconds": true
    });

    $('input[name="OperationsDashboard_AuditTrailDatatable_DateRange"]').on('cancel.daterangepicker', function (ev, picker) {
        // clearing an input
        $('input[name="OperationsDashboard_AuditTrailDatatable_DateRange"]').val('');
    });

    $('input[name="OperationsDashboard_AuditTrailDatatable_DateRange"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format(dateFormat) + ' - ' + picker.endDate.format(dateFormat));
    });
});