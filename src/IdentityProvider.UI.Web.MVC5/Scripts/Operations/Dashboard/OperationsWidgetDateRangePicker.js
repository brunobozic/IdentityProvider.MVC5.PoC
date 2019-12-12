var dateFormat = 'DD.MM.YYYY HH:MM:SS';

$(function () {
    $('input[name="OperationsDashboard_OperationsWidget_DateRange"]').daterangepicker({
        timePicker: true,
        startDate: moment().startOf('hour').add(-1,'month'),
        endDate: moment().startOf('hour').add(24, 'hour'),
        autoUpdateInput: false,
        locale: {
            format: dateFormat,
            cancelLabel: 'Clear'
        },
        "minYear": 2000,
        "maxYear": 2050,
        "timePicker24Hour": true,
        "timePickerSeconds": true,
        //onChange: function() {
        //    $("#DateRangePickerOnOperationsStart").val(JSON.parse($(this).val()).start);
        //    $("#DateRangePickerOnOperationsEnd").val(JSON.parse($(this).val()).end);
        //}
    }, function (start, end, label) {
        //$(this).val(start.format(dateFormat) + ' - ' + end.format(dateFormat));
        //$('#DateRangePickerOnOperationsStartHidden').val(start);
        //$('#DateRangePickerOnOperationsEndHidden').val(end);
    });

    $('input[name="OperationsDashboard_OperationsWidget_DateRange"]').on('cancel.daterangepicker', function (ev, picker) {
        // clearing an input

        $('input[name="OperationsDashboard_OperationsWidget_DateRange"]').val('');
        $('#DateRangePickerOnOperationsStartHidden').val('');
        $('#DateRangePickerOnOperationsEndHidden').val('');
    });

    $('input[name="OperationsDashboard_OperationsWidget_DateRange"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format(dateFormat) + ' - ' + picker.endDate.format(dateFormat));

        $('#DateRangePickerOnOperationsStartHidden').val(picker.startDate.format(dateFormat));
        $('#DateRangePickerOnOperationsEndHidden').val(picker.endDate.format(dateFormat));
    });

    $('input[name="OperationsDashboard_OperationsWidget_DateRange"]').on('show.daterangepicker', function (ev, picker) {
        // change the selected date range of that picker
        var start = $('#DateRangePickerOnOperationsStartHidden').val();
        var end = $('#DateRangePickerOnOperationsEndHidden').val();
        if (start) {
            picker.setStartDate(start);
        }
        if (end) {
            picker.setEndDate(end);
        }
        if (start && end) {
            $(this).val(picker.startDate.format(dateFormat) + ' - ' + picker.endDate.format(dateFormat));
        }
    });

});