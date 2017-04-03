$(document).on('click', '.btn-add-user-mail', function (e) {
    e.preventDefault();

    var controlForm = $('.user-mail-controls:first'),
        currentEntry = $(this).parents('.user-mail-entry:first'),
        newEntry = $(currentEntry.clone()).appendTo(controlForm);

    newEntry.find('input').val('');
    controlForm.find('.user-mail-entry:not(:last) .btn-add')
        .removeClass('btn-add-user-mail').addClass('btn-remove')
        .removeClass('btn-success').addClass('btn-danger')
        .html('<span class="fa fa-minus"></span>');
}).on('click', '.btn-remove', function (e) {
    $(this).parents('.user-mail-entry:first').remove();

    e.preventDefault();
    return false;
});