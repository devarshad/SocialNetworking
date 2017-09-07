/**
 * Custom jquery.validation defaults
 */
$.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).addClass(errorClass).removeClass(validClass);
        } else {
            $(element).addClass(errorClass).removeClass(validClass);
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        }
    },
    unhighlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).removeClass(errorClass).addClass(validClass);
        } else {
            $(element).removeClass(errorClass).addClass(validClass);
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        }
    },
    showErrors: function (errorMap, errorList) {
        this.defaultShowErrors();
        // If an element is valid, it doesn't need a tooltip
        $("." + this.settings.validClass).tooltip("destroy");

        // Add tooltips
        for (var i = 0; i < errorList.length; i++) {
            var error = errorList[i];
            //var id = '#' + error.element.id;
            //var isInModal = $(id).parents('.modal').length > 0;
            //, container: isInModal, html: false }) // Activate the tooltip on focus
            $(error.element).tooltip({ trigger: "focus" })
                .attr('data-tooltip', 'tooltip-danger')
                .attr("data-original-title", error.message);
        }
    }
});