$(function () {
    skinChanger();
    activateNotificationAndTasksScroll();

    setSkinListHeightAndScroll(true);
    setSettingListHeightAndScroll(true);
    $(window).resize(function () {
        setSkinListHeightAndScroll(false);
        setSettingListHeightAndScroll(false);
    });
});

//Skin changer
function skinChanger() {
    $('.right-sidebar .demo-choose-skin li').on('click', function () {
        var $body = $('body');
        var $this = $(this);

        var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
        $('.right-sidebar .demo-choose-skin li').removeClass('active');
        $body.removeClass('theme-' + existTheme);
        $this.addClass('active');
        $body.addClass('theme-' + $this.data('theme'));


        try {
            var $buttons = $('button');
            var $select = $('select');

            $buttons.removeClass('themed-buttons-' + 'pink');
            $buttons.removeClass('bg-' + 'pink');
            $buttons.removeClass('themed-buttons-' + 'red');
            $buttons.removeClass('bg-' + 'red');
            $buttons.removeClass('themed-buttons-' + existTheme);
            $buttons.removeClass('bg-' + existTheme);
            $buttons.addClass('themed-buttons-' + $this.data('theme'));
            $buttons.addClass('bg-' + $this.data('theme'));

            $select.removeClass('themed-buttons-' + existTheme);
            $select.removeClass('bg-' + existTheme);
            $select.addClass('themed-buttons-' + $this.data('theme'));
            $select.addClass('bg-' + $this.data('theme'));

            var $paginatorn = $('#SearchResultTable_next');
            $paginatorn.removeClass('themed-buttons-' + 'pink');
            $paginatorn.removeClass('bg-' + 'pink');
            $paginatorn.removeClass('themed-buttons-' + existTheme);
            $paginatorn.removeClass('bg-' + existTheme);
            $paginatorn.addClass('themed-buttons-' + $this.data('theme'));
            $paginatorn.addClass('bg-' + $this.data('theme'));

            var $paginatorl = $('#SearchResultTable_last');
            $paginatorl.removeClass('themed-buttons-' + 'pink');
            $paginatorl.removeClass('bg-' + 'pink');
            $paginatorl.removeClass('themed-buttons-' + existTheme);
            $paginatorl.removeClass('bg-' + existTheme);
            $paginatorl.addClass('themed-buttons-' + $this.data('theme'));
            $paginatorl.addClass('bg-' + $this.data('theme'));

            var $paginatorf = $('#SearchResultTable_first');
            $paginatorf.removeClass('themed-buttons-' + 'pink');
            $paginatorf.removeClass('bg-' + 'pink');
            $paginatorf.removeClass('themed-buttons-' + existTheme);
            $paginatorf.removeClass('bg-' + existTheme);
            $paginatorf.addClass('themed-buttons-' + $this.data('theme'));
            $paginatorf.addClass('bg-' + $this.data('theme'));

            var $paginatorprev = $('#SearchResultTable_previous');
            $paginatorprev.removeClass('themed-buttons-' + 'pink');
            $paginatorprev.removeClass('bg-' + 'pink');
            $paginatorprev.removeClass('themed-buttons-' + existTheme);
            $paginatorprev.removeClass('bg-' + existTheme);
            $paginatorprev.addClass('themed-buttons-' + $this.data('theme'));
            $paginatorprev.addClass('bg-' + $this.data('theme'));

            $.fn.dataTable.ext.classes.sLengthSelect = 'btn bg-' + $this.data('theme');               // Change Pagination Button Class
            $.fn.dataTable.ext.classes.sPageButton = 'btn bg-' + $this.data('theme');                 // Change Pagination Button Class
            $.fn.dataTable.ext.classes.sPageButtonActive = 'btn bg-' + $this.data('theme');           // Change Pagination Button Class

            var $spana = $('span >a');
            $spana.removeClass('themed-buttons-' + 'pink');
            $spana.removeClass('bg-' + 'pink');
            $spana.removeClass('themed-buttons-' + existTheme);
            $spana.removeClass('bg-' + existTheme);
            $spana.addClass('themed-buttons-' + $this.data('theme'));
            $spana.addClass('bg-' + $this.data('theme'));

            var $select2Selection = $('.select2-selection__choice');
            
            // TODO: need to reapply this on page redraw
            $select2Selection.attr('style', 'background-color: blue !important');

        } catch (exception) {
            console.log("operationEditPartial editDialog onClick problem", exception);
        }
    });
}

//Skin tab content set height and show scroll
function setSkinListHeightAndScroll(isFirstTime) {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.demo-choose-skin');

    if (!isFirstTime) {
        $el.slimScroll({ destroy: true }).height('auto');
        $el.parent().find('.slimScrollBar, .slimScrollRail').remove();
    }

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '6px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Setting tab content set height and show scroll
function setSettingListHeightAndScroll(isFirstTime) {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.right-sidebar .demo-settings');

    if (!isFirstTime) {
        $el.slimScroll({ destroy: true }).height('auto');
        $el.parent().find('.slimScrollBar, .slimScrollRail').remove();
    }

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '6px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Activate notification and task dropdown on top right menu
function activateNotificationAndTasksScroll() {
    $('.navbar-right .dropdown-menu .body .menu').slimscroll({
        height: '254px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Google Analiytics ======================================================================================
addLoadEvent(loadTracking);
var trackingId = 'UA-30038099-6';

function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
}

function loadTracking() {
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

    ga('create', trackingId, 'auto');
    ga('send', 'pageview');
}
//========================================================================================================
