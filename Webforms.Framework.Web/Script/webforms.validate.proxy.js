(function ($) {
    if (window.ValidatorUpdateDisplay) {
        var proxied = window.ValidatorUpdateDisplay;

        window.ValidatorUpdateDisplay = function () {
            onBefore(arguments);
            var result = proxied.apply(this, arguments);
            onAfter(arguments);
            return result;
        };

        var onBefore = function (arguments) {
        };

        var onAfter = function (arguments) {
            var control = document.getElementById(arguments[0].controltovalidate);
            var validators = control.Validators;
            var isValid = true;

            for (var i = 0; i < validators.length; i++) {
                if (!validators[i].isvalid) {
                    isValid = false;
                    break;
                }
            }

            if (isValid) {
                $(control).removeClass('error');
            } else {
                $(control).addClass('error');
            }
        };
    }
})(jQuery);