// Replaces the default Jquery "serialize" and "serializeArray" with an implementation that knows how to return the state of
// a checkbox as "True" or "False" instead of as "on" and "off" which we cannot use on server side....
(function ($) {
    $.fn.serialize = function (options) {
        return $.param(this.serializeArray(options));
    };

    $.fn.serializeArray = function (options) {
        var o = $.extend({
            checkboxesAsBools: false
        },
            options || {});

        var rselectTextarea = /select|textarea/i;
        var rinput = /text|hidden|password|search/i;

        return this.map(function () {
            return this.elements ? $.makeArray(this.elements) : this;
        })
            .filter(function () {
                return this.name &&
                    !this.disabled &&
                    (this.checked ||
                        (o.checkboxesAsBools && this.type === "checkbox") ||
                        rselectTextarea.test(this.nodeName) ||
                        rinput.test(this.type));
            })
            .map(function (i, elem) {
                var val = $(this).val();
                return val == null
                    ? null
                    : $.isArray(val)
                        ? $.map(val,
                            function (val, i) {
                                return { name: elem.name, value: val };
                            })
                        : {
                            name: elem.name,
                            value: (o.checkboxesAsBools && this.type === "checkbox")
                                ? (this.checked ? "true" : "false")
                                : val
                        };
            }).get();
    };
})(jQuery);