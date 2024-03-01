// ======== Datatables wireup ========
$(document).ready(function () {
    var existTheme = $(".right-sidebar .demo-choose-skin li.active").data("theme");
    console.log("painting paginator: " + existTheme);
    $.fn.dataTable.ext.classes.sLengthSelect = "btn btn-xs bg-" + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButton = "btn btn-xs bg-" + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButtonActive = "btn btn-xs bg-" + existTheme; // Change Pagination Button Class

    //// Setup - add a text input to each footer cell
    //$('#SearchResultTable tfoot th').each( function () {
    //    var title = $(this).text();
    //    $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
    //});

    var datatableName = "#OperationsDashboard_AuditTrailDatatable";
    var dateFormat = "DD.MM.YYYY HH:MM:SS"; // the default date format used

    var auditTrailDatatable = $(datatableName).DataTable({
        dom: "Brltip",
        colReorder: true, // allow for column reordering
        "columnDefs": [
            { "width": "2%", "visible": false, "targets": [0] }, // Id
            { "width": "2%", "searchable": true, "orderable": true, "targets": [1] }, // TableName
            { "width": "2%", "searchable": true, "orderable": true, "targets": [2] }, // Action
            { "width": "5px", "searchable": false, "orderable": true, "targets": [3] }, // UpdatedAt
            { "width": "5px", "searchable": true, "orderable": true, "targets": [4] }, // OldData
            { "width": "5px", "searchable": true, "orderable": true, "targets": [5] }, // NewData
            { "width": "5px", "searchable": true, "orderable": true, "targets": [6] }, // UserName
            { "className": "text-center custom-middle-align", "targets": [] },
            { "className": "id-column", "targets": [0] }
        ],
        select: {
            style: "multi"
        },
        buttons: [ // add the export capabilities (dependent on several js modules)
            {
                extend: "copyHtml5",
                title: "Data Export"
            },
            {
                extend: "excelHtml5",
                title: "Data Export",
                text: '<i class="fa fa-file-excel-o"></i>',
                titleAttr: "Excel"
            },
            {
                extend: "csvHtml5",
                title: "Data Export",
                text: '<i class="fa fa-file-text-o"></i>',
                titleAttr: "CSV"
            },
            {
                extend: "pdfHtml5",
                title: "Data Export",
                text: '<i class="fa fa-file-pdf-o"></i>',
                titleAttr: "PDF"
            }
        ],
        "autoWidth": false,
        "bAutoWidth": false,
        "keys": true,
        "fixedHeader": true,
        "sortable": true, // allow sorting
        processing: true, // show a loading gif while processing data
        "serverSide": true, // use server side endpoint for fetching data
        rowReorder: {
            selector: "td:nth-child(2)"
        },
        responsive: true, // shrinks with available realestate
        "pageLength": 10, // default value for how many items are shown in paginator scenarios
        "pagingType": "full_numbers",
        "ajax": {
            url: datatablesConString,
            type: "POST",
            datatype: "json",
            "data": function (data) {
                // Read the date start and date end from hidden form elements
                var start = $("#DateRangePickerOnAuditTrailStartHidden").val();
                var end = $("#DateRangePickerOnAuditTrailEndHidden").val();

                data.from = start;
                data.to = end;

                data.search_userName = $("#OperationsDashboard_AuditTrailDatatable_SubmitSearchUserName").val();
                data.search_oldValue = $("#OperationsDashboard_AuditTrailDatatable_SubmitSearchOldValue").val();
                data.search_newValue = $("#OperationsDashboard_AuditTrailDatatable_SubmitSearchNewValue").val();

                data.tables = $("#OperationsDashboard_AuditTrailDatatable_TableName_MS").val();
                console.log("OperationsDashboard_AuditTrailDatatable_TableName_MS: " +
                    $("#OperationsDashboard_AuditTrailDatatable_TableName_MS").val());
                data.actions = $("#OperationsDashboard_AuditTrailDatatable_Actions_MS").val();
                console.log("OperationsDashboard_AuditTrailDatatable_Actions_MS: " +
                    $("#OperationsDashboard_AuditTrailDatatable_Actions_MS").val());

                // This I use to re-color the paginator buttons according to the currently selected theme color
                var existTheme = $(".right-sidebar .demo-choose-skin li.active").data("theme");

                $.fn.dataTable.ext.classes.sLengthSelect =
                    "btn btn-xs bg-" + existTheme; // Change Pagination Button Class
                $.fn.dataTable.ext.classes.sPageButton =
                    "btn btn-xs bg-" + existTheme; // Change Pagination Button Class
                $.fn.dataTable.ext.classes.sPageButtonActive =
                    "btn btn-xs bg-" + existTheme; // Change Pagination Button Class
                console.log("painting paginator: " + existTheme);
            }
        },
        lengthMenu: [5, 10, 20, 50, 100, 200, 500],
        "language": {
            "search": "",
            "searchPlaceholder": " Search...",
            loadingRecords:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="/img/ajaxloading.gif"></div>', // The loading gif is hardcoded in this manner
            processing:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="/img/ajaxloading.gif"></div>'  // The loading gif is hardcoded in this manner
        },
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": false, "responsivePriority": 1000 },
            { "data": "TableName", "name": "TableName", "autoWidth": false, "responsivePriority": 10 },
            { "data": "Action", "name": "Action", "autoWidth": false, "responsivePriority": 10 },
            {
                "data": "UpdatedAt",
                "name": "UpdatedAt",
                "autoWidth": false,
                "responsivePriority": 10, // Zhis prioritizes which columns get rotated and hidden when horizontal realestate shrinks
                type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat); // This is how you override how a certain data type will end up being rendered
                }
            },
            { "data": "OldData", "name": "OldData", "autoWidth": false, "responsivePriority": 1900 },
            { "data": "NewData", "name": "NewData", "autoWidth": false, "responsivePriority": 1800 },
            { "data": "UserName", "name": "UserName", "autoWidth": false, "responsivePriority": 1000 }
        ],
        "drawCallback": function (settings) {
            console.log('AuditTrailDatatableWireup => myMuuriGrid.refreshItems().layout()');

            myMuuriGrid.refreshItems().layout();

            // This I use to re-color the paginator buttons according to the currently selected theme color
            // perhaps an unnecessary complication but here it is...
            var existTheme = $(".right-sidebar .demo-choose-skin li.active").data("theme");

            var $paginatorn = $("#OperationsDashboard_AuditTrailDatatable_next");
            $paginatorn.removeClass("themed-buttons-" + "pink");
            $paginatorn.removeClass("bg-" + "pink");
            $paginatorn.removeClass("themed-buttons-" + existTheme);
            $paginatorn.removeClass("bg-" + existTheme);
            $paginatorn.addClass("themed-buttons-" + existTheme);
            $paginatorn.addClass("bg-" + existTheme);

            var $paginatorl = $("#OperationsDashboard_AuditTrailDatatable_last");
            $paginatorl.removeClass("themed-buttons-" + "pink");
            $paginatorl.removeClass("bg-" + "pink");
            $paginatorl.removeClass("themed-buttons-" + existTheme);
            $paginatorl.removeClass("bg-" + existTheme);
            $paginatorl.addClass("themed-buttons-" + existTheme);
            $paginatorl.addClass("bg-" + existTheme);

            var $paginatorf = $("#OperationsDashboard_AuditTrailDatatable_first");
            $paginatorf.removeClass("themed-buttons-" + "pink");
            $paginatorf.removeClass("bg-" + "pink");
            $paginatorf.removeClass("themed-buttons-" + existTheme);
            $paginatorf.removeClass("bg-" + existTheme);
            $paginatorf.addClass("themed-buttons-" + existTheme);
            $paginatorf.addClass("bg-" + existTheme);

            var $paginatorprev = $("#OperationsDashboard_AuditTrailDatatable_previous");
            $paginatorprev.removeClass("themed-buttons-" + "pink");
            $paginatorprev.removeClass("bg-" + "pink");
            $paginatorprev.removeClass("themed-buttons-" + existTheme);
            $paginatorprev.removeClass("bg-" + existTheme);
            $paginatorprev.addClass("themed-buttons-" + existTheme);
            $paginatorprev.addClass("bg-" + existTheme);

            var $paginatornOp = $("#OperationsDashboard_OperationsDatatable_next");
            $paginatornOp.removeClass("themed-buttons-" + "pink");
            $paginatornOp.removeClass("bg-" + "pink");
            $paginatornOp.removeClass("themed-buttons-" + existTheme);
            $paginatornOp.removeClass("bg-" + existTheme);
            $paginatornOp.addClass("themed-buttons-" + existTheme);
            $paginatornOp.addClass("bg-" + existTheme);

            var $paginatorlOp = $("#OperationsDashboard_OperationsDatatable_last");
            $paginatorlOp.removeClass("themed-buttons-" + "pink");
            $paginatorlOp.removeClass("bg-" + "pink");
            $paginatorlOp.removeClass("themed-buttons-" + existTheme);
            $paginatorlOp.removeClass("bg-" + existTheme);
            $paginatorlOp.addClass("themed-buttons-" + existTheme);
            $paginatorlOp.addClass("bg-" + existTheme);

            var $paginatorfOp = $("#OperationsDashboard_OperationsDatatable_first");
            $paginatorfOp.removeClass("themed-buttons-" + "pink");
            $paginatorfOp.removeClass("bg-" + "pink");
            $paginatorfOp.removeClass("themed-buttons-" + existTheme);
            $paginatorfOp.removeClass("bg-" + existTheme);
            $paginatorfOp.addClass("themed-buttons-" + existTheme);
            $paginatorfOp.addClass("bg-" + existTheme);

            var $paginatorprevOp = $("#OperationsDashboard_OperationsDatatable_previous");
            $paginatorprevOp.removeClass("themed-buttons-" + "pink");
            $paginatorprevOp.removeClass("bg-" + "pink");
            $paginatorprevOp.removeClass("themed-buttons-" + existTheme);
            $paginatorprevOp.removeClass("bg-" + existTheme);
            $paginatorprevOp.addClass("themed-buttons-" + existTheme);
            $paginatorprevOp.addClass("bg-" + existTheme);
            console.log("painting paginator: " + existTheme);

            var $spana = $("span > a");
            $spana.removeClass("themed-buttons-" + "pink");
            $spana.removeClass("bg-" + "pink");
            $spana.removeClass("themed-buttons-" + "red");
            $spana.removeClass("bg-" + "red");
            $spana.removeClass("themed-buttons-" + existTheme);
            $spana.removeClass("bg-" + existTheme);
            $spana.addClass("themed-buttons-" + existTheme);
            $spana.addClass("bg-" + existTheme);
        },
        "initComplete": function (settings, json) {
        }
    });

    $(document).ready(function () {
        $(datatableName).change(function () {
        });
    });

    $("#OperationsDashboard_AuditTrailDatatable_SubmitSearchUserName").on("click",
        function (e) {
            auditTrailDatatable.draw();
        });
    //// Extend dataTables search
    //$.fn.dataTable.ext.search.push(
    //    function(settings, data, dataIndex) {
    //        var min = $('#min-date').val();
    //        var max = $('#max-date').val();
    //        var createdAt = data[2] || 0; // Our date column in the table

    //        if (
    //            (min == "" || max == "") ||
    //                (moment(createdAt).isSameOrAfter(min) && moment(createdAt).isSameOrBefore(max))
    //        ) {
    //            return true;
    //        }
    //        return false;
    //    }
    //);

    //$('#my-table_filter').hide();
});