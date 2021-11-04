
try {
    myMuuriGrid = new Muuri(".grid",
        {
            layout: {
                fillGaps: false,
                horizontal: false,
                alignRight: false,
                alignBottom: false,
                rounding: false
            },
            dragStartPredicate: function (item, event) {
                // only allow drag and drop if the handle is being used
                var isRemoveAction = elementMatches(event.target, ".handleForDragandDrop");

                if (isRemoveAction == false) {
                    return false;
                }

                return Muuri.ItemDrag.defaultStartPredicate(item, event);
            },
            dragReleaseDuration: 600,
            dragReleseEasing: "ease",
            dragEnabled: true,
            layoutOnResize: true
        });
} catch (exception) {
    console.log("muuri exception", exception);
}

// Helper method, returns whether an element matches a given selector, this is supposed to be the most performant variant
function elementMatches(element, selector) {
    var p = Element.prototype;

    return (p.matches ||
        p.matchesSelector ||
        p.webkitMatchesSelector ||
        p.mozMatchesSelector ||
        p.msMatchesSelector ||
        p.oMatchesSelector).call(element, selector);
}