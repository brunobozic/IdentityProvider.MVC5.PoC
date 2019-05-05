// Delete item from grid
function itemDelete(id) {
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
}