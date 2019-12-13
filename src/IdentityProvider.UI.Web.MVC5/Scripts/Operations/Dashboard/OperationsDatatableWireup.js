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

    var datatableName = '#OperationsDashboard_OperationsDatatable';
    var dateFormat = 'DD.MM.YYYY HH:MM:SS';

    var operationsDatatable = $(datatableName).DataTable({
        dom: 'Brltip',
        colReorder: true,
        "columnDefs": [
            { "width": "5%", "visible": true, "targets": [0] }, // Id
            { "width": "10%", "searchable": false, "orderable": true, "targets": [1] }, // Name
            { "width": "10%", "searchable": false, "orderable": true, "targets": [2] }, // Description
            { "width": "10%", "searchable": false, "orderable": true, "targets": [3] }, // Active
            { "width": "10%", "searchable": false, "orderable": true, "targets": [4] }, // CreatedDate
            { "width": "20%", "searchable": false, "orderable": true, "targets": [5] }, // ModifiedDate
            { "width": "20%", "searchable": false, "orderable": true, "targets": [6] }, // Actions
            { "className": "text-center custom-middle-align", "targets": [3, 4, 5, 6] }
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
        "autoWidth": false,
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
            url: datatablesConStringForOperations,
            type: 'POST',
            datatype: "json",
            "data": function (data) {
                var start = $('#DateRangePickerOnOperationsStartHidden').val();
                var end = $('#DateRangePickerOnOperationsEndHidden').val();
                data.from = start;
                data.to = end;
                data.show_inactive = true;
                data.show_deleted = false;
                data.search_extra = $('#SearchStringOperationsMainGrid').val();
                skinChange();
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search...",
            loadingRecords:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="http://www.snacklocal.com/images/ajaxload.gif"></div>',
            processing:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="http://www.snacklocal.com/images/ajaxload.gif"></div>'
        },
        lengthMenu: [5, 10, 20, 50, 100, 200, 500],
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
            { "data": "Active", "name": "Active", "autoWidth": true },
            {
                "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true, type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat);
                }
            },
            {
                "data": "ModifiedDate", "name": "ModifiedDate", "autoWidth": true, type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat);
                }
            },
            {
                data: null,
                className: "center",
                defaultContent: '<a href="" class="OperationsDashboard_OperationsDatatable_edit">Edit</a> / <a href="" class="OperationsDashboard_OperationsDatatable_remove">Delete</a>'
            }
        ],
        "drawCallback": function (settings) {
            myMuuriGrid.refreshItems().layout();
            skinChange();
        },
        "initComplete": function (settings, json) {
            skinChange();
        }
    });

    $(document).ready(function () {
        $('#SearchResultTable_length').change(function () {

        });
    });

    // New record
    $('a.OperationsDashboard_OperationsDatatable_create').on('click', function (e) {
        e.preventDefault();
        var options = { keyboard: true, focus: true };
        $.ajax({
            type: "GET",
            url: insertUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html(response);

                $('#mdModal').on('shown.bs.modal', function () {
                });
            },
            failure: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').find('.modal-body').html("Problem loading your data...");

                $('#mdModal').on('shown.bs.modal', function () {

                });

            },
            error: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').find('.modal-body').html("Problem loading your data...");

                $('#mdModal').on('shown.bs.modal', function () {

                });
            }
        });
    });

    // Edit record
    $('#OperationsDashboard_OperationsDatatable').on('click', 'a.OperationsDashboard_OperationsDatatable_edit', function (e) {
        e.preventDefault();

        var id = $(this).closest('tr').children('td:first').text();
        var options = { /*'backdrop': 'static',*/ keyboard: true, focus: true };

        $.ajax({
            type: "GET",
            url: editUrl + '?id=' + id,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html(response);
            },
            failure: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("Problem loading your data...");
            },
            error: function (response) {
                $('#mdModal').modal(options);
                $('#mdModal').find('.modal-body').html("");
                $('#mdModal').modal('show');
                $('#mdModal').find('.modal-body').html("Problem loading your data...");
            }
        });
    });

    // Delete a record
    $('#OperationsDashboard_OperationsDatatable').on('click', 'a.OperationsDashboard_OperationsDatatable_remove', function (e) {
        e.preventDefault();
        var id = $(this).closest('tr').children('td:first').text();
        var options = { /*'backdrop': 'static',*/ keyboard: true, focus: true };

        console.log("id: " + id + "  , deleteUrl: " + deleteUrl);


        swal.setDefaults({
            buttonsStyling: false,
            confirmButtonClass: 'btn btn-success w-25 mr-05',
            cancelButtonClass: 'btn btn-secondary w-25 ml-05'
        });
        swal({
            title: "Are you sure?",
            text: "Are you sure that you want to delete this item?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            confirmButtonColor: "#ec6c62"
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: deleteUrl,
                    data:
                    {
                        "itemToDelete": id
                    },
                    type: "POST",
                    //contentType: "application/json; charset=utf-8",
                    //dataType: "json",
                    success: successFunc,
                    error: errorFunc
                });
                function successFunc(data, status) {

                    swal(
                        'Deleted!',
                        'Your item has been deleted.',
                        'success'
                    ).then(function () {
                        location.reload();
                    });;

                }
                function errorFunc() {

                    swal(
                        'Nothing changed!',
                        'Your item has not been deleted.',
                        'error'
                    ).then(function () {
                        location.reload();
                    });;
                }

            } else {
                swal(
                    'Nothing changed!',
                    'Your item has not been deleted.',
                    'error'
                );
            }
        });
    });

    $('#searchStringOperationsMainGrid').on('click', function (e) {
        operationsDatatable.draw();
    });


    // Extend dataTables search
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = $('#min-date').val();
            var max = $('#max-date').val();
            var createdAt = data[2] || 0; // Our date column in the table

            if (
                (min === "" || max === "") ||
                (moment(createdAt).isSameOrAfter(min) && moment(createdAt).isSameOrBefore(max))
            ) {
                return true;
            }
            return false;
        }
    );

    //$('#my-table_filter').hide();

});

