
    try {
         myMuuriGrid = new Muuri('.grid',
            {
                layout: {
                    fillGaps: false,
                    horizontal: false,
                    alignRight: false,
                    alignBottom: false,
                    rounding: false
                },
                dragStartPredicate: function (item, event) {
                    var isRemoveAction = elementMatches(event.target, '.handleForDragandDrop');
               
                    if (isRemoveAction == false) {
                        return false;
                    }

                    return Muuri.ItemDrag.defaultStartPredicate(item, event);
                },
                dragReleaseDuration: 600,
                dragReleseEasing: 'ease',
                dragEnabled: true,
                layoutOnResize: true
            });
    } catch (exception) {
        console.log('muuri exception', exception);
    }


function elementMatches(element, selector) {
    var p = Element.prototype;
  
    return (p.matches || p.matchesSelector || p.webkitMatchesSelector || p.mozMatchesSelector || p.msMatchesSelector || p.oMatchesSelector).call(element, selector);
}