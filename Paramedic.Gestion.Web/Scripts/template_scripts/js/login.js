$(function () {

    jQuery.support.placeholder = false;
    test = document.createElement('input');
    if ('placeholder' in test) jQuery.support.placeholder = true;

    if (!$.support.placeholder) {

        $('.field').find('label').show();

    }

    $('#forgotPassword').on('click', function () {

        var $msg = "<p><i class='icon-envelope' style='margin-right:5px;color:#00ba8b !important'></i>Por favor, ingrese el email registrado.</p> " +
                    "<div class='form-group'>" +
                        "<input class='form-control' id='emailForRecovery' name='emailForRecovery' type='text' autofocus=''>" +
                    "</div>";

        bootbox.dialog({
            message: $msg,
            title: "Recuperación de Password",
            buttons: {
                danger: {
                    label: "Aceptar",
                    className: "btn btn-info",
                    callback: function () {

                        var $email = $("#emailForRecovery").val();

                        $.ajax({
                            url: "/Account/RecoverPassword",
                            datatype: "json",
                            traditional: true,
                            data: { 'email': $email },
                            type: 'POST',
                            success: function (vSuccess) {
                                vSuccess = parseInt(vSuccess);
                                //console.log(vSuccess);
                                $('#emailForRecovery').val("");
                                var $msg = "";
                                if (vSuccess == 1) {
                                    $msg = "Su nuevo password ha sido enviado al email ingresado.";
                                } else {
                                    $msg = "El email ingresado no es correcto.";
                                }

                                alert($msg);
                            },
                            error: function (error) {
                                alert("Error. No se pudo enviar la petición");
                            }
                        });
                    }
                },
                main: {
                    label: "Cancelar",
                    className: "btn btn-danger",
                    callback: function () {

                    }
                }
            }
        });

    });

});

