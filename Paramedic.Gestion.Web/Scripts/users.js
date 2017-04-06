$(document).ready(function () {

    $(document).on('click', '.btn-add-user-mail', function (e) {
        e.preventDefault();

        var controlForm = $('.user-mail-controls:first'),
            currentEntry = $(this).parents('.user-mail-entry:first'),
            currentContainer = $(this).parents('.user-mail-container:first'),
            currentError = currentContainer.find('p.user-mail-error');

        var currentValue = currentEntry.find('input').val();

        if (!validateEmail(currentValue)) {
            currentError.show();
            return false;
        }

        currentError.hide();
        var newEntry = $(currentContainer.clone()).appendTo(controlForm);
        $('span[data-valmsg-for="Emails"]').hide();

        newEntry.find('input').val('');
        controlForm.find('.user-mail-entry:not(:last) .btn-add-user-mail')
            .removeClass('btn-add-user-mail').addClass('btn-remove')
            .removeClass('btn-success').addClass('btn-danger')
            .html('<span class="fa fa-minus"></span>');
        newEntry.find('input').focus();
    }).on('click', '.btn-remove', function (e) {
        $(this).parents('.user-mail-entry:first').remove();

        e.preventDefault();
        return false;
    });

    function validateEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

});