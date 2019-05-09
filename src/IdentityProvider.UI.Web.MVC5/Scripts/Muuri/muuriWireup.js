$(document).ready(function () {
    try {
        var grid = new Muuri('.grid',
            {
                layout: {
                    fillGaps: false,
                    horizontal: false,
                    alignRight: false,
                    alignBottom: false,
                    rounding: true
                },
                dragStartPredicate: function (item, event) { 
                    // select2-search__field
                    // sorting
                    // dataTables_paginate
                    // fa
                    // material-icon
                    var isRemoveAction = elementMatches(event.target, '.select2-selection__rendered, .sorting, .dataTables_paginate, .fa, .material-icon, .select2-search__field');
                   
                    if (isRemoveAction) {
                        return false;
                    }

                   return Muuri.ItemDrag.defaultStartPredicate(item, event) ;
                },
                dragReleaseDuration: 600,
                dragReleseEasing: 'ease',
                dragEnabled: true,
                layoutOnResize: true
            });
    } catch (exception) {
        console.log('muuri exception', exception);
    }
});

function elementMatches(element, selector) {
    var p = Element.prototype;
    console.log('Element.prototype: ' + Element.prototype);
    return (p.matches || p.matchesSelector || p.webkitMatchesSelector || p.mozMatchesSelector || p.msMatchesSelector || p.oMatchesSelector).call(element, selector);
}