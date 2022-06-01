$(function() {
    skinChanger();
    activateNotificationAndTasksScroll();

    setSkinListHeightAndScroll(true);
    setSettingListHeightAndScroll(true);
    $(window).resize(function() {
        setSkinListHeightAndScroll(false);
        setSettingListHeightAndScroll(false);
    });
});

function skinChange() {
    var existTheme = $(".right-sidebar .demo-choose-skin li.active").data("theme");

    console.log("Current theme: " + existTheme);

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

    $.fn.dataTable.ext.classes.sLengthSelect = "btn bg-" + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButton = "btn bg-" + existTheme; // Change Pagination Button Class
    $.fn.dataTable.ext.classes.sPageButtonActive = "btn bg-" + existTheme; // Change Pagination Button Class

    var $buttons = $("button:not(.dt-button)");
    $buttons.removeClass("themed-buttons-" + "pink");
    $buttons.removeClass("bg-" + "pink");
    $buttons.removeClass("themed-buttons-" + "red");
    $buttons.removeClass("bg-" + "red");
    $buttons.removeClass("themed-buttons-" + existTheme);
    $buttons.removeClass("bg-" + existTheme);
    $buttons.addClass("themed-buttons-" + existTheme);
    $buttons.addClass("bg-" + existTheme);

    var $spana = $("span > a");
    $spana.removeClass("themed-buttons-" + "pink");
    $spana.removeClass("bg-" + "pink");
    $spana.removeClass("themed-buttons-" + existTheme);
    $spana.removeClass("bg-" + existTheme);
    $spana.addClass("themed-buttons-" + existTheme);
    $spana.addClass("bg-" + existTheme);

    var $select2Selection = $(".select2-selection__choice");

    // TODO: need to reapply this on page redraw
    $select2Selection.attr("style", "background-color: " + $this.data("theme") + " !important");


    var $checkboxes = $(":checkbox");
    $checkboxes.removeClass("chk-col-" + "red");
    $checkboxes.addClass("chk-col-" + existTheme);
}

//Skin changer
function skinChanger() {
    $(".right-sidebar .demo-choose-skin li").on("click",
        function() {
            var $body = $("body");
            var $this = $(this);

            var existTheme = $(".right-sidebar .demo-choose-skin li.active").data("theme");

            console.log("Current theme: " + existTheme);
            $(".right-sidebar .demo-choose-skin li").removeClass("active");
            $body.removeClass("theme-" + existTheme);
            $this.addClass("active");
            $body.addClass("theme-" + $this.data("theme"));


            try {
                var $buttons = $("button:not(.dt-button)");
                var $select = $("select");

                $buttons.removeClass("themed-buttons-" + "pink");
                $buttons.removeClass("bg-" + "pink");
                $buttons.removeClass("themed-buttons-" + "red");
                $buttons.removeClass("bg-" + "red");
                $buttons.removeClass("themed-buttons-" + existTheme);
                $buttons.removeClass("bg-" + existTheme);
                $buttons.addClass("themed-buttons-" + $this.data("theme"));
                $buttons.addClass("bg-" + $this.data("theme"));

                $select.removeClass("themed-buttons-" + existTheme);
                $select.removeClass("bg-" + existTheme);
                $select.addClass("themed-buttons-" + $this.data("theme"));
                $select.addClass("bg-" + $this.data("theme"));

                var $paginatorn = $("#OperationsDashboard_AuditTrailDatatable_next");
                $paginatorn.removeClass("themed-buttons-" + "pink");
                $paginatorn.removeClass("bg-" + "pink");
                $paginatorn.removeClass("themed-buttons-" + existTheme);
                $paginatorn.removeClass("bg-" + existTheme);
                $paginatorn.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorn.addClass("bg-" + $this.data("theme"));

                var $paginatorl = $("#OperationsDashboard_AuditTrailDatatable_last");
                $paginatorl.removeClass("themed-buttons-" + "pink");
                $paginatorl.removeClass("bg-" + "pink");
                $paginatorl.removeClass("themed-buttons-" + existTheme);
                $paginatorl.removeClass("bg-" + existTheme);
                $paginatorl.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorl.addClass("bg-" + $this.data("theme"));

                var $paginatorf = $("#OperationsDashboard_AuditTrailDatatable_first");
                $paginatorf.removeClass("themed-buttons-" + "pink");
                $paginatorf.removeClass("bg-" + "pink");
                $paginatorf.removeClass("themed-buttons-" + existTheme);
                $paginatorf.removeClass("bg-" + existTheme);
                $paginatorf.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorf.addClass("bg-" + $this.data("theme"));

                var $paginatorprev = $("#OperationsDashboard_AuditTrailDatatable_previous");
                $paginatorprev.removeClass("themed-buttons-" + "pink");
                $paginatorprev.removeClass("bg-" + "pink");
                $paginatorprev.removeClass("themed-buttons-" + existTheme);
                $paginatorprev.removeClass("bg-" + existTheme);
                $paginatorprev.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorprev.addClass("bg-" + $this.data("theme"));

                var $paginatornOp = $("#OperationsDashboard_OperationsDatatable_next");
                $paginatornOp.removeClass("themed-buttons-" + "pink");
                $paginatornOp.removeClass("bg-" + "pink");
                $paginatornOp.removeClass("themed-buttons-" + existTheme);
                $paginatornOp.removeClass("bg-" + existTheme);
                $paginatornOp.addClass("themed-buttons-" + $this.data("theme"));
                $paginatornOp.addClass("bg-" + $this.data("theme"));

                var $paginatorlOp = $("#OperationsDashboard_OperationsDatatable_last");
                $paginatorlOp.removeClass("themed-buttons-" + "pink");
                $paginatorlOp.removeClass("bg-" + "pink");
                $paginatorlOp.removeClass("themed-buttons-" + existTheme);
                $paginatorlOp.removeClass("bg-" + existTheme);
                $paginatorlOp.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorlOp.addClass("bg-" + $this.data("theme"));

                var $paginatorfOp = $("#OperationsDashboard_OperationsDatatable_first");
                $paginatorfOp.removeClass("themed-buttons-" + "pink");
                $paginatorfOp.removeClass("bg-" + "pink");
                $paginatorfOp.removeClass("themed-buttons-" + existTheme);
                $paginatorfOp.removeClass("bg-" + existTheme);
                $paginatorfOp.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorfOp.addClass("bg-" + $this.data("theme"));

                var $paginatorprevOp = $("#OperationsDashboard_OperationsDatatable_previous");
                $paginatorprevOp.removeClass("themed-buttons-" + "pink");
                $paginatorprevOp.removeClass("bg-" + "pink");
                $paginatorprevOp.removeClass("themed-buttons-" + existTheme);
                $paginatorprevOp.removeClass("bg-" + existTheme);
                $paginatorprevOp.addClass("themed-buttons-" + $this.data("theme"));
                $paginatorprevOp.addClass("bg-" + $this.data("theme"));

                $.fn.dataTable.ext.classes.sLengthSelect =
                    "btn bg-" + $this.data("theme"); // Change Pagination Button Class
                $.fn.dataTable.ext.classes.sPageButton =
                    "btn bg-" + $this.data("theme"); // Change Pagination Button Class
                $.fn.dataTable.ext.classes.sPageButtonActive =
                    "btn bg-" + $this.data("theme"); // Change Pagination Button Class

                var $spana = $("span > a");
                $spana.removeClass("themed-buttons-" + "pink");
                $spana.removeClass("bg-" + "pink");
                $spana.removeClass("themed-buttons-" + "red");
                $spana.removeClass("bg-" + "red");
                $spana.removeClass("themed-buttons-" + existTheme);
                $spana.removeClass("bg-" + existTheme);
                $spana.addClass("themed-buttons-" + $this.data("theme"));
                $spana.addClass("bg-" + $this.data("theme"));

                var $select2Selection = $(".select2-selection__choice");

                // TODO: need to reapply this on page redraw
                $select2Selection.attr("style", "background-color: " + $this.data("theme") + " !important");

                var $checkboxes = $(":checkbox");
                $checkboxes.removeClass("chk-col-" + "red");
                $checkboxes.addClass("chk-col-" + $this.data("theme"));

            } catch (exception) {
                console.log("operationEditPartial editDialog onClick problem", exception);
            }
        });
}

// Skin tab content set height and show scroll
function setSkinListHeightAndScroll(isFirstTime) {
    var height = $(window).height() - ($(".navbar").innerHeight() + $(".right-sidebar .nav-tabs").outerHeight());
    console.log("navbar inner: " + $(".navbar").innerHeight());
    console.log("navbar outer: " + $(".navbar").outerHeight());
    console.log(height);
    var $el = $(".demo-choose-skin");
    debugger;
    if (!isFirstTime) {
        $el.slimScroll({ destroy: true }).height("auto");
        $el.parent().find(".slimScrollBar, .slimScrollRail").remove();
        debugger;
        $el.parents().find(".right-sidebar").css("top", height);
        $el.slimscroll({
            height: height + "px",
            color: "rgba(0,0,0,0.5)",
            size: "6px",
            alwaysVisible: false,
            borderRadius: "0",
            railBorderRadius: "0"
        });
    }

    // Setting tab content set height and show scroll
    function setSettingListHeightAndScroll(isFirstTime) {
        var height = $(window).height() - ($(".navbar").innerHeight() + $(".right-sidebar .nav-tabs").outerHeight());
        console.log("navbar inner: " + $(".navbar").innerHeight());
        console.log("navbar outer: " + $(".navbar").outerHeight());
        console.log(height);
        debugger;
        var $el = $(".right-sidebar .demo-settings");

        if (!isFirstTime) {
            $el.slimScroll({ destroy: true }).height("auto");
            $el.parent().find(".slimScrollBar, .slimScrollRail").remove();
        }
        debugger;
        $el.parents().find(".right-sidebar").css("top", height);
        $el.slimscroll({
            height: height + "px",
            color: "rgba(0,0,0,0.5)",
            size: "6px",
            alwaysVisible: false,
            borderRadius: "0",
            railBorderRadius: "0"
        });
    }

    // Activate notification and task dropdown on top right menu
    function activateNotificationAndTasksScroll() {
        $(".navbar-right .dropdown-menu .body .menu").slimscroll({
            height: "254px",
            color: "rgba(0,0,0,0.5)",
            size: "4px",
            alwaysVisible: false,
            borderRadius: "0",
            railBorderRadius: "0"
        });
    }
}