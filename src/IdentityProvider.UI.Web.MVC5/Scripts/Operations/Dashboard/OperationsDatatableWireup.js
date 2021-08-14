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
            { "width": "2%", "visible": false, "targets": [0] }, // Id
            { "width": "2%", "searchable": true, "orderable": true, "targets": [1] }, // Name
            { "width": "2%", "searchable": true, "orderable": true, "targets": [2] }, // Description
            { "width": "2%", "searchable": false, "orderable": true, "targets": [3] }, // Active
            { "width": "2%", "searchable": false, "orderable": true, "targets": [4] }, // Deleted
            { "width": "5px", "searchable": false, "orderable": true, "targets": [5] }, // CreatedDate
            { "width": "5px", "searchable": false, "orderable": true, "targets": [6] }, // ModifiedDate
            { "width": "5px", "searchable": false, "orderable": false, "targets": [7] }, // Actions
            { "className": "text-center custom-middle-align", "targets": [] },
            { "className": "id-column", "targets": [0] }
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
        "bAutoWidth": false,
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
                data.alsoinactive = $('#ShowDeleted').is(':checked');
                data.alsodeleted = $('#ShowInactive').is(':checked');
                data.search_extra = $('#searchStringOperationsMainGrid').val();
            }
        },
        "language": {
            "search": "",
            "searchPlaceholder": "Search...",
            loadingRecords:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="/Content/img/ajaxloading.gif"></div>',
            processing:
                '<div style="width:100%; z-index: 11000 !important; text-align: center;"><img src="/Content/img/ajaxloading.gif"></div>'
        },
        lengthMenu: [5, 10, 20, 50, 100, 200, 500],
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": false, "responsivePriority": 1000 },
            { "data": "Name", "name": "Name", "autoWidth": false, "responsivePriority": 10 },
            { "data": "Description", "name": "Description", "autoWidth": false, "responsivePriority": 100 },
            {
                "data": "Active", "name": "Active", "autoWidth": false, "responsivePriority": 10,
                "render": function (data, type, row) {
                    return (data === true) ? '<span class="glyphicon glyphicon-ok"></span>' : '<span class="glyphicon glyphicon-remove" ></span>';
                }
            },
            {
                "data": "Deleted", "name": "Deleted", "autoWidth": false, "responsivePriority": 10,
                "render": function (data, type, row) {
                    return (data === true) ? '<i class="material-icons" style="color:red" data-toggle="tooltip" data-placement="top" title="This item was deleted by user: [Guest] at [DateDeleted]">delete_forever </i>' : ' ';
                }
            },
            {
                "data": "CreatedDate", "name": "CreatedDate", "autoWidth": false, "responsivePriority": 1900, type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat);
                }
            },
            {
                "data": "ModifiedDate", "name": "ModifiedDate", "autoWidth": false, "responsivePriority": 10, type: "datetime",
                render: function (data, type, row) {
                    return moment(data).format(dateFormat);
                }
            },
            { "data": "Actions", "name": "Actions", "autoWidth": false, "responsivePriority": 2000 },
            //{
            //    data: null,
            //    className: "center",
            //    defaultContent: '<a href="" class="OperationsDashboard_OperationsDatatable_edit text-center custom-middle-align">Edit</a> / <a href="" class="OperationsDashboard_OperationsDatatable_remove">Delete</a>'
            //}    
        ],
        "drawCallback": function (settings) {
            myMuuriGrid.refreshItems().layout();
            var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');

            var $paginatorn = $('#OperationsDashboard_AuditTrailDatatable_next');
            $paginatorn.removeClass('themed-buttons-' + 'pink');
            $paginatorn.removeClass('bg-' + 'pink');
            $paginatorn.removeClass('themed-buttons-' + existTheme);
            $paginatorn.removeClass('bg-' + existTheme);
            $paginatorn.addClass('themed-buttons-' + existTheme);
            $paginatorn.addClass('bg-' + existTheme);

            var $paginatorl = $('#OperationsDashboard_AuditTrailDatatable_last');
            $paginatorl.removeClass('themed-buttons-' + 'pink');
            $paginatorl.removeClass('bg-' + 'pink');
            $paginatorl.removeClass('themed-buttons-' + existTheme);
            $paginatorl.removeClass('bg-' + existTheme);
            $paginatorl.addClass('themed-buttons-' + existTheme);
            $paginatorl.addClass('bg-' + existTheme);

            var $paginatorf = $('#OperationsDashboard_AuditTrailDatatable_first');
            $paginatorf.removeClass('themed-buttons-' + 'pink');
            $paginatorf.removeClass('bg-' + 'pink');
            $paginatorf.removeClass('themed-buttons-' + existTheme);
            $paginatorf.removeClass('bg-' + existTheme);
            $paginatorf.addClass('themed-buttons-' + existTheme);
            $paginatorf.addClass('bg-' + existTheme);

            var $paginatorprev = $('#OperationsDashboard_AuditTrailDatatable_previous');
            $paginatorprev.removeClass('themed-buttons-' + 'pink');
            $paginatorprev.removeClass('bg-' + 'pink');
            $paginatorprev.removeClass('themed-buttons-' + existTheme);
            $paginatorprev.removeClass('bg-' + existTheme);
            $paginatorprev.addClass('themed-buttons-' + existTheme);
            $paginatorprev.addClass('bg-' + existTheme);

            var $paginatornOp = $('#OperationsDashboard_OperationsDatatable_next');
            $paginatornOp.removeClass('themed-buttons-' + 'pink');
            $paginatornOp.removeClass('bg-' + 'pink');
            $paginatornOp.removeClass('themed-buttons-' + existTheme);
            $paginatornOp.removeClass('bg-' + existTheme);
            $paginatornOp.addClass('themed-buttons-' + existTheme);
            $paginatornOp.addClass('bg-' + existTheme);

            var $paginatorlOp = $('#OperationsDashboard_OperationsDatatable_last');
            $paginatorlOp.removeClass('themed-buttons-' + 'pink');
            $paginatorlOp.removeClass('bg-' + 'pink');
            $paginatorlOp.removeClass('themed-buttons-' + existTheme);
            $paginatorlOp.removeClass('bg-' + existTheme);
            $paginatorlOp.addClass('themed-buttons-' + existTheme);
            $paginatorlOp.addClass('bg-' + existTheme);

            var $paginatorfOp = $('#OperationsDashboard_OperationsDatatable_first');
            $paginatorfOp.removeClass('themed-buttons-' + 'pink');
            $paginatorfOp.removeClass('bg-' + 'pink');
            $paginatorfOp.removeClass('themed-buttons-' + existTheme);
            $paginatorfOp.removeClass('bg-' + existTheme);
            $paginatorfOp.addClass('themed-buttons-' + existTheme);
            $paginatorfOp.addClass('bg-' + existTheme);

            var $paginatorprevOp = $('#OperationsDashboard_OperationsDatatable_previous');
            $paginatorprevOp.removeClass('themed-buttons-' + 'pink');
            $paginatorprevOp.removeClass('bg-' + 'pink');
            $paginatorprevOp.removeClass('themed-buttons-' + existTheme);
            $paginatorprevOp.removeClass('bg-' + existTheme);
            $paginatorprevOp.addClass('themed-buttons-' + existTheme);
            $paginatorprevOp.addClass('bg-' + existTheme);
            console.log('painting paginator: ' + existTheme);

            var $spana = $('span > a');
            $spana.removeClass('themed-buttons-' + 'pink');
            $spana.removeClass('bg-' + 'pink');
            $spana.removeClass('themed-buttons-' + 'red');
            $spana.removeClass('bg-' + 'red');
            $spana.removeClass('themed-buttons-' + existTheme);
            $spana.removeClass('bg-' + existTheme);
            $spana.addClass('themed-buttons-' + existTheme);
            $spana.addClass('bg-' + existTheme);

        },
        "initComplete": function (settings, json) {

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

        var id = $(this).data('id');

        console.log("id: " + id + "  , editUrl: " + editUrl);

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
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
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

    $('#submitSearchOperationsMainGrid').on('click', function (e) {
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

