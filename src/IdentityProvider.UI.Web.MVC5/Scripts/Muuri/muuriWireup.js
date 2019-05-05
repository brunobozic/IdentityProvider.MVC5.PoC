$(document).ready(function () {
    try {
        var grid = new Muuri('.grid',
            {
                dragEnabled: true
            });
    } catch (exception) {
        console.log('muuri exception', exception);
    }
});