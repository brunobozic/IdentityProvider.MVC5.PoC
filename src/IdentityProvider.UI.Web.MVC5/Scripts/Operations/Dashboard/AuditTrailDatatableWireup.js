// ======== Datatables wireup ========
$(document).ready(function () {
    var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
    $.fn.dataTable.ext.classes.sLengthSelect = 'btn btn-xs bg-' + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButton = 'btn btn-xs bg-' + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButtonActive = 'btn btn-xs bg-' + existTheme; // Change Pagination Button Class

    //// Setup - add a text input to each footer cell
    //$('#SearchResultTable tfoot th').each( function () {
    //    var title = $(this).text();
    //    $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
    //});

    var datatableName = '#OperationsDashboard_AuditTrailDatatable';
    var dateFormat = 'DD.MM.YYYY HH:MM:SS';

    var auditTrailDatatable = $(datatableName).DataTable({
        dom: 'Brltip',
        colReorder: true,
        "columnDefs": [
            { "width": "5%", "visible": false, "targets": [0] },                        // Id 
            { "width": "5%", "searchable": true, "orderable": true, "targets": [1] },  // TableName  
            { "width": "5%", "searchable": true, "orderable": true, "targets": [2] },  // Action
            { "width": "5%", "searchable": false, "orderable": true, "targets": [3] }, // UpdatedAt
            { "width": "5%", "searchable": true, "orderable": true, "targets": [4] },  // OldData
            { "width": "5%", "searchable": true, "orderable": true, "targets": [5] },  // NewData  
            { "width": "5%", "searchable": true, "orderable": true, "targets": [6] },  // UserName  
            { "className": "text-center custom-middle-align", "targets": [] }
        ],
        select: {
            style: 'multi'
        },
        buttons: [
            {
                extend: 'copyHtml5',
                title: 'Data Export'
            },
            {
                extend: 'excelHtml5',
                title: 'Data Export',
                text: '<i class="fa fa-file-excel-o"></i>',
                titleAttr: 'Excel'
            },
            {
                extend: 'csvHtml5',
                title: 'Data Export',
                text: '<i class="fa fa-file-text-o"></i>',
                titleAttr: 'CSV'
            },
            {
                extend: 'pdfHtml5',
                title: 'Data Export',
                text: '<i class="fa fa-file-pdf-o"></i>',
                titleAttr: 'PDF'
            }
        ],
        "autoWidth": true,
        "keys": true,
        "fixedHeader": true,
        "sortable": true,
        processing: true,
        "serverSide": true,
        rowReorder: {
            selector: 'td:nth-child(2)'
        },
        responsive: true,
        "pageLength": 10,
        "pagingType": "full_numbers",
        "ajax": {
            url: datatablesConString,
            type: 'POST',
            datatype: "json",
            "data": function (data) {
                var start = $('#DateRangePickerOnAuditTrailStartHidden').val();
                var end = $('#DateRangePickerOnAuditTrailEndHidden').val();
                data.from = start;
                data.to = end;

                data.search_userName = $('#OperationsDashboard_AuditTrailDatatable_SubmitSearchUserName').val();
                data.search_oldValue = $('#OperationsDashboard_AuditTrailDatatable_SubmitSearchOldValue').val();
                data.search_newValue = $('#OperationsDashboard_AuditTrailDatatable_SubmitSearchNewValue').val();
                skinChange();
            }
        },
        lengthMenu: [5, 10, 20, 50, 100, 200, 500],
        "language": {
            "search": "",
            "searchPlaceholder": " Search...",
            loadingRecords:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="~/Content/img/ajaxloading.gif"></div>',
            processing:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="~/Content/img/ajaxloading.gif"></div>'
        },
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            { "data": "TableName", "name": "TableName", "autoWidth": true },
            { "data": "Action", "name": "Action", "autoWidth": true },
            {
                "data": "UpdatedAt",
                "name": "UpdatedAt",
                "autoWidth": true,
                type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat);
                }
            },
            { "data": "OldData", "name": "OldData", "autoWidth": true },
            { "data": "NewData", "name": "NewData", "autoWidth": true },
            { "data": "UserName", "name": "UserName", "autoWidth": true }
        ],
        "drawCallback": function (settings) {
            myMuuriGrid.refreshItems().layout();
            skinChange();
        },
        "initComplete": function (settings, json) {
            var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
            skinChange();
        }
    });

    $(document).ready(function () {
        $(datatableName).change(function () {
            skinChange();
        });
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

