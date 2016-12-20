
$(function () {

    //en la tabla ClientesGestiones, corto el texto si es mayor a 20 caracteres
    function setFormatTables() {

        $("#tbGestion > tbody > tr > td").each(function () {

            var str = $(this).html().trim();

            if (str.length > 70) {

                if (str.indexOf("a href") == -1) {
                    var subStr = str.substring(0, 70);
                    $(this).html(subStr + "...");
                }
            }
        });

    }

    var closeTicket = function () {

        var $a = $(this);
        confirmCloseTicket($a);
        return false;

    }

    var ticketEvento = function () {

        var $btn = $(this);
        var $url = $btn.attr("data-ref");
        var $evID = $btn.attr("data-evento-id");
        var $ticketID = $btn.attr("data-model-id");
        var $dataResponse = $btn.attr("data-response");
        $dataResponse = parseInt($dataResponse);
        confirmTicketEvento($url, $evID, $btn, $dataResponse, $ticketID);
        return false;

    }

    function confirmTicketEvento($url, $evID, $btn, $dataResponse, $ticketID) {

        var $message = "";
        var $title = "";
        var $tipoEvento = 1;

        if ($evID == undefined) { //Creacion de consulta

            $message = "<form id=\"formAddConsulta\" action=\"/MisTickets/CreateTicketEvento/" + $ticketID + "\" enctype=\"multipart/form-data\" method=\"post\"><textarea class='form-control' rows='10' name='descripcion' id='descripcion' style='width:100%' placeholder='Ingrese su consulta...' autofocus='true' /><br/><input type=\"file\" id=\"image\" name=\"image\" accept=\"image/x-png, image/gif, image/jpeg\" /><input type=\"hidden\" id=\"tipoEvento\" name=\"tipoEvento\" value=\"1\" /></form>";
            $title = "<span class='label label-primary'>Nueva consulta</span>";
        } else if ($dataResponse == 1) { //Creacion de respuesta

            $message = "<form id=\"formAddRespuesta\" action=\"/MisTickets/CreateTicketEvento/" + $ticketID + "\" enctype=\"multipart/form-data\" method=\"post\"><textarea class='form-control' rows='10' name='descripcion' id='descripcion' style='width:100%' placeholder='Ingrese la respuesta...' autofocus='true' /><br/><input type=\"file\" id=\"image\" name=\"image\" accept=\"image/x-png, image/gif, image/jpeg\" /><input type=\"hidden\" id=\"tipoEvento\" name=\"tipoEvento\" value=\"2\" /></form>";
            $title = "<span class='label label-primary'>Respuesta de consulta</span>";
        } else { //Edición consulta/respuesta

            $description = $btn.closest('.alert').find('p').text();
            $message = "<form id=\"formEditAll\" action=\"/MisTickets/EditTicketEvento/" + $evID + "\" enctype=\"multipart/form-data\" method=\"post\"><textarea class='form-control' rows='10' name='descripcion' id='descripcion' style='width:100%' autofocus='true' >" + $description + "</textarea><br/><input type=\"file\" id=\"image\" name=\"image\" accept=\"image/x-png, image/gif, image/jpeg\" /></form>";
            $title = "<span class='label label-primary'>Editar</span>";

        }

        bootbox.dialog({
            message: $message,
            className: 'modalTickets',
            title: $title,
            buttons: {
                danger: {
                    label: "Aceptar",
                    className: "btn-primary",
                    callback: function () {
                        if ($evID == undefined) {
                            $('form#formAddConsulta').submit();
                        } else if ($dataResponse == 1) {
                            $('form#formAddRespuesta').submit();
                        } else {
                            $('form#formEditAll').submit();
                        }

                        //$.ajax({
                        //    url: $url,
                        //    data: {
                        //        'descripcion': $("#descripcion").val(),
                        //        'tipoEvento': $tipoEvento
                        //    },
                        //    type: 'POST',
                        //    success: function (data) {
                        //        var target = "#listaEventos";
                        //        $(target).replaceWith(data);
                        //    },
                        //    error: function (error) {
                        //        alert(error.responseText);
                        //    }
                        //});
                    }
                },
                main: {
                    label: "Cancelar",
                    className: "btn-primary",
                    callback: function () {

                    }
                }
            }
        });

    }

    function confirmCloseTicket($a) {

        bootbox.dialog({
            message: "¿Está seguro que desea cerrar el ticket?",
            title: "Atención!",
            buttons: {
                danger: {
                    label: "Aceptar",
                    className: "btn-danger",
                    callback: function () {
                        var $url = $a.attr("href");
                        $.ajax({
                            url: $url,
                            type: 'POST',
                            success: function (data) {
                                var target = "#bottomEditTicket";
                                $(target).
                                    fadeOut(500, function () { $(target).html(data).fadeIn(500); });
                                $('.btnEditCreateEvento').remove();
                            },
                            error: function (error) {
                                alert(error.responseText);
                            }
                        });
                    }
                },
                main: {
                    label: "Cancelar",
                    className: "btn-primary",
                    callback: function () {

                    }
                }
            }
        });
    }

    // Seteo popup modal para confirmar si borro un registro de la tabla (general)
    function setDeleteConfirmBox($a) {

        bootbox.dialog({
            message: "¿Está seguro que desea eliminar el registro?",
            title: "Atención!",
            buttons: {
                danger: {
                    label: "Aceptar",
                    className: "btn-danger",
                    callback: function () {
                        var $url = $a.attr("href");
                        blockInterface();
                        $.ajax({
                            url: $url,
                            type: 'POST',
                            success: function (data) {
                                var target = $a.attr("data-gestion-target");
                                $(target).replaceWith(data);
                                $.unblockUI();
                            },
                            error: function (error) {
                                alert(error.responseText);
                                $.unblockUI();
                            }
                        });
                    }
                },
                main: {
                    label: "Cancelar",
                    className: "btn-info",
                    callback: function () {

                    }
                }
            }
        });
    }

    // Obtengo un vector con todos los modulos excluidos
    function getModulosExcluidos() {

        var vModExc = [];
        $checkboxes = $('#modulosList div.switch-on');

        for (var i = 0; i < $checkboxes.length; i++) {
            vModExc.push($checkboxes[i].firstChild.id.toString());
        }

        return vModExc;

    }

    //Seteo via ajax los modulos excluidos de un producto.
    function setModulosExcluidos($a) {

        var $url = $a.attr("href");

        $.ajax({
            url: $url,
            type: 'GET',
            success: function (data) {
                bootbox.dialog({
                    message: data,
                    title: "Seleccione módulos a excluir",
                    buttons: {
                        danger: {
                            label: "Aceptar",
                            className: "btn-danger",
                            callback: function () {
                                var $url = $a.attr("data-gestion-setModExc");
                                var $modExc = getModulosExcluidos();
                                $.ajax({
                                    url: $url,
                                    datatype: "json",
                                    traditional: true,
                                    data: { 'vModExc': $modExc },
                                    type: 'POST',
                                    success: function (data) {

                                    },
                                    error: function (error) {
                                        alert(error.statusText);
                                    }
                                });
                            }
                        },
                        main: {
                            label: "Cancelar",
                            className: "btn-info",
                            callback: function () {

                            }
                        }
                    }
                });

                $('.make-switch').each(function (index, elem) {
                    //Initialize all switches if they haven't been already
                    if (!$(elem).hasClass('has-switch')) {
                        $(elem).bootstrapSwitch();
                    }
                });
            },
            error: function (error) {
                alert(error.statusText);
            }
        });
    }

    // Handler para cerrar sesion
    var cerrarSesion = function () {
        $('#logoutForm').submit();
    }

    // Handler para la vista de clientes del dropdown vendidos / en gestion
    var selTipoClientes = function () {

        $tipoCliente = $(this).attr("value");
        $url = "/Clientes/GetInfoClienteSegunEstado/" + $tipoCliente;
        $.ajax({
            url: $url,
            type: 'GET',
            success: function (data) {
                var target = "#clientesList";
                $(target).replaceWith(data);
            },
            error: function (error) {
                alert(error.statusText);
            }
        });

    }

    var ajaxFormSubmit = function () {

        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };

        blockInterface();

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-gestion-target"));
            var $newHtml = $(data);
            $target.replaceWith($newHtml);
            setFormatTables();
            $.unblockUI();

        });

        return false;
    };

    var getPage = function () {
        var $a = $(this);
        var options = {
            url: $a.attr("href"),
            data: $("form").serialize(),
            type: "get"
        };

        blockInterface();

        $.ajax(options).done(function (data) {
            var target = $a.parents("div.pagedList").attr("data-gestion-target");
            $(target).replaceWith(data);
            setFormatTables();
            $.unblockUI();
        });

        return false;
    };

    $(document).on('click', '.delete', function () {

        var $a = $(this);

        setDeleteConfirmBox($a);

        return false;

    });

    $(document).on('click', '.modExcluidos', function () {

        var $a = $(this);

        setModulosExcluidos($a);

        return false;

    });

    var execPrincipalForm = function () {

        $('.searchIndex').submit();

    }

    function validarLocalidad() {

        var $obj = $(".selectLoc[data-validar-localidad='true']");
        var $loc_id = $obj.val();
        var $url = "/Clientes/ValidarLocalidad/" + $loc_id;
        $.ajax({
            url: $url,
            type: 'GET',
            success: function (data) {
                if (data != "") {
                    var arr = [];
                    arr = data.split("&");
                    $('#paisCliente').val(arr[1]);
                    $('#provCliente').val(arr[0]);
                }
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });

    }

    function blockInterface() {

        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: 'Aguarde un instante...'
        });

    }

    $('.new_model').tooltip({ placement: "top" });

    $('#cerrarSesion').click(cerrarSesion);

    $("form[data-gestion-ajax='true']").submit(ajaxFormSubmit);

    $(".selectpicker[data-validar-localidad='true']").on('change', validarLocalidad);

    $(document).on("click", ".pagedList a", getPage);

    $('#selTipoClientes,#selDatosSegunVista,#selTipoGestion').on('change', execPrincipalForm);

    $('#btnCloseTicket').on('click', closeTicket);

    $(document).on('click', '.btnEditCreateEvento', ticketEvento);

    $("a.single_image").fancybox({
        'type': 'image',
        'showCloseButton': 'true',
    });

    $('.selectpicker').selectpicker({
        style: 'btn-primary',
        width: '100%'
    });

    // Seteo el formato de los datepickers
    $(".datepicker").datepicker({ autoclose: true, format: 'dd/mm/yyyy' });


    $("#tituloShaman")
        .mouseover(function () {
            $(this).addClass('animated tada');
        })
        .mouseout(function () {
            $(this).removeClass('animated tada');
        });

    $("ul#navOptions.nav.navbar-nav.side-nav > li > a")
        .mouseover(function () {
            $(this).addClass('animated pulse');
        })
        .mouseout(function () {
            $(this).removeClass('animated pulse');
        });

    var verifPrivacidadVideo = function () {

        if ($('#esPublico').is(':checked')) {

            $('select[name="vidCliente"]').attr('disabled', 'disabled');
            $('.btn.selectpicker').attr('disabled', 'disabled');

        } else {

            $('select[name="vidCliente"]').removeAttr('disabled');
            $('.btn.selectpicker').removeAttr('disabled');
        }

    };

    verifPrivacidadVideo();
    $('#esPublico').on('click', verifPrivacidadVideo);

    //validarLocalidad();
    setFormatTables();

    $(".various").on('click', function () {

        var vid_id = $(this).attr("href").replace('#','');
        console.log(vid_id);

        $.ajax({
            type: 'POST',
            data: { idVideo: vid_id },
            url: "/LogsRegistrosSistema/SetVideoLog",
            success: function (data) {
            },
            error: function (data) {
            }
        });
    });


    $(".various").fancybox({
        maxWidth: 800,
        maxHeight: 600,
        fitToView: false,
        width: '640px',
        height: '264px',
        autoSize: false,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none'
    });

    function setViewOnMap() {

        console.log(vMediosDifusion);

        $.ajax({
            type: 'GET',
            data: { vData: vMediosDifusion },
            traditional: true,
            dataType: 'json',
            url: "/Mapa/GetPositionsOfClients",
            success: function (positions) {
                if (positions != null) {
                    setMarkers(positions);
                } else {
                    clearOverlays();
                }

            }
        });
    }

    $(document.body).on('change', '.chkMediosDifusion', function () {

        var $option = $(this).parents()[1].children[1];
        var idMedioDifusion = parseInt($(this).val());

        if ($(this).is(':checked')) {
            vMediosDifusion.push(idMedioDifusion);
            $option.style.background = "#D7ECDD";

        } else {

            vMediosDifusion.splice($.inArray(idMedioDifusion, vMediosDifusion), 1);
            $option.style.background = null;

        }
        setViewOnMap();
    });

    //Cambio el filtro de tipo de cliente en el mapa

    var gmarkers = [];
    var vMediosDifusion = [];
    var infowindow;

    if (window.location.pathname == "/Mapa") {
        vMediosDifusion = [1, 2];
        $('.chkMediosDifusion').each(function () {
            var $option = $(this).parents()[1].children[1];
            if ($(this).val() == 1 || $(this).val() == 2) {
                $(this).prop('checked', true);
                $option.style.background = "#D7ECDD";
            }
        });
        setViewOnMap();
        infowindow = new google.maps.InfoWindow({
            content: ""
        });
    }


    //Seteo los marcadores para el filtro de tipo de cliente seleccionado.
    function setMarkers(posiciones) {

        clearOverlays();

        var latitud, longitud, imagen, shadow, htmlInfo, punto, cliente;

        for (var i = 0; i < posiciones.length ; i++) {

            latitud = posiciones[i].Latitud;
            latitud = parseFloat(latitud);
            longitud = posiciones[i].Longitud;
            longitud = parseFloat(longitud);
            punto = new google.maps.LatLng(latitud, longitud);
            createMarker(punto, i, posiciones[i]);

        }
    }

    function createMarker(punto, idx, vInfoCliente) {

        var marker = new google.maps.Marker({
            position: punto,
            map: map,
            zIndex: 1
        });

        //Cambio color de icono dependiendo si esta en gestión o vendido.

        switch (vInfoCliente.MedioDifusionID) {
            case 1:
                marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter_withshadow&chld=G|e74c3c|ecf0f1');
                break;
            case 2:
                marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter_withshadow&chld=V|2ecc71|ecf0f1');
                break;
            case 3:
                marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter_withshadow&chld=S|f1c40f|ecf0f1');
                break;
            case 4:
                marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter_withshadow&chld=D|95a5a6|ecf0f1');
                break;
            default:

        }


        gmarkers[idx] = marker;

        addInfoWindow(marker, vInfoCliente);

    }

    function clearOverlays() {

        for (var i = 0; i < gmarkers.length ; i++) {

            gmarkers[i].setMap(null);

        }
    }

    function addInfoWindow(marker, vInfoCliente) {

        if (vInfoCliente.SitioWeb == null) {
            vInfoCliente.SitioWeb = 'Sin información';
        }

        //    var contentString = '<ul id="tabs" class="nav nav-tabs" data-tabs="tabs">' +
        //    '<li class="active"><a href="#red" data-toggle="tab">Red</a></li>' +
        //    '<li><a href="#orange" data-toggle="tab">Orange</a></li>' +
        //    '<li><a href="#yellow" data-toggle="tab">Yellow</a></li>' +
        //    '<li><a href="#green" data-toggle="tab">Green</a></li>' +
        //    '<li><a href="#blue" data-toggle="tab">Blue</a></li>' +
        //'</ul>' +
        //'<div id="my-tab-content" class="tab-content">' +
        //    '<div class="tab-pane active" id="red">' +
        //        '<h1>Red</h1>' +
        //        '<p>red red red red red red</p>' +
        //    '</div>' +
        //    '<div class="tab-pane" id="orange">' +
        //        '<h1>Orange</h1>' +
        //        '<p>orange orange orange orange orange</p>' +
        //    '</div>' +
        //    '<div class="tab-pane" id="yellow">' +
        //        '<h1>Yellow</h1>' +
        //        '<p>yellow yellow yellow yellow yellow</p>' +
        //    '</div>' +
        //    '<div class="tab-pane" id="green">' +
        //        '<h1>Green</h1>' +
        //        '<p>green green green green green</p>' +
        //    '</div>' +
        //    '<div class="tab-pane" id="blue">' +
        //        '<h1>Blue</h1>' +
        //        '<p>blue blue blue blue blue</p>' +
        //    '</div>' +
        //'</div>';

        var contentString = '<div id="content">' +
              '<div id="siteNotice">' +
              '</div>' +
              '<h3 id="firstHeading" class="firstHeading">' + vInfoCliente.Cliente + '</h3>' +
              '<div id="bodyContent">' +
              '<p>Localidad: ' + vInfoCliente.Localidad + ' </p>' +
              '<p>Email: ' + vInfoCliente.EmailPrincipal + ' </p>' +
              '<p>Tel&eacute;fono: ' + vInfoCliente.Telefono + ' </p>' +
              '<p>Sitio Web: ' + vInfoCliente.SitioWeb + ' </p>' +
              '<p><a href="/Clientes/Edit/' + vInfoCliente.ID + '">Ver m&aacute;s</a></p>' +
              '</div>' +
              '</div>';


        google.maps.event.addListener(marker, 'click', function () {
            if (infowindow) {
                infowindow.close();
            }

            infowindow.setContent(contentString);
            infowindow.open(map, marker);
        });
    }

    $('#FuturaMejora').click(function () {
        blockInterface();
        $.ajax({
            url: '/MisTickets/SetFutureFeature',
            datatype: "json",
            traditional: true,
            data: {
                'isFutureFeature': $(this).is(":checked"),
                'ticketId' : $('#ID').val()
            },
            type: 'POST',
            success: function (data) {
                console.log(data);
                $.unblockUI();
            },
            error: function (error) {
                alert(error.statusText);
                $.unblockUI();
            }
        });
    });

});

