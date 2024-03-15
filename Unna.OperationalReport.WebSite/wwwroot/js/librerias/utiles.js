// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function realizarJsonGet(url, datos, fSuceso, fError, timeoutAjax, noRedireccionar) {
    realizarGet(url, datos, 'json', fSuceso, fError, timeoutAjax, noRedireccionar);
}

function realizarGet(url, datos, tipoRespuesta, fSuceso, fError, timeoutAjax, noRedireccionar) {

    var timeoutDefecto = 1000 * 60;
    if (timeoutAjax) {
        timeoutDefecto = 1000 * timeoutAjax;
    }
    //console.log(timeoutDefecto);

    var xhr = $.ajax({
        type: "GET",
        url: url,
        dataType: tipoRespuesta,
        data: datos,
        success: function (data) {
            if (fSuceso) {
                fSuceso(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
            desbloquearPantalla();

            if (jqXHR.status == 403) {
                alert("Su sesión acaba de expirar. Por favor ingrese de nuevo su login");
                var respuestaRedireccion = JSON.parse(jqXHR.responseText);
                location.href = respuestaRedireccion.urlLogin;
            } else if (jqXHR.status == 401) {
                location.reload();
                return;
            }
            if (fError) {
                fError(jqXHR, textStatus, errorThrown);
            }

        },
        timeout: timeoutDefecto
    });
    return xhr;
}

function realizarJsonDelete(url, datos, fSuceso, fError, timeoutAjax, noRedireccionar) {
    realizarDelete(url, datos, 'json', fSuceso, fError, timeoutAjax, noRedireccionar);
}

function realizarDelete(url, datos, tipoRespuesta, fSuceso, fError, timeoutAjax, noRedireccionar) {

    var timeoutDefecto = 1000 * 60;
    if (timeoutAjax) {
        timeoutDefecto = 1000 * timeoutAjax;
    }
    //console.log(timeoutDefecto);

    var xhr = $.ajax({
        type: "DELETE",
        url: url,
        dataType: tipoRespuesta,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datos),
        success: function (data) {
            if (fSuceso) {
                fSuceso(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
            desbloquearPantalla();

            if (jqXHR.status == 403) {
                alert("Su sesión acaba de expirar. Por favor ingrese de nuevo su login");
                var respuestaRedireccion = JSON.parse(jqXHR.responseText);
                location.href = respuestaRedireccion.urlLogin;
            } else if (jqXHR.status == 401) {
                location.reload();
                return;
            }

            if (fError) {
                fError(jqXHR, textStatus, errorThrown);
            }

        },
        timeout: timeoutDefecto
    });
    return xhr;
}



function realizarJsonPost(url, datos, fSuceso, fError, timeoutAjax, noRedireccionar) {
    realizarPost(url, datos, 'json', fSuceso, fError, timeoutAjax, noRedireccionar);
}

function realizarPost(url, datos, tipoRespuesta, fSuceso, fError, timeoutAjax, noRedireccionar) {

    var timeoutDefecto = 1000 * 60;
    if (timeoutAjax) {
        timeoutDefecto = 1000 * timeoutAjax;
    }
    //console.log(timeoutDefecto);

    var xhr = $.ajax({
        type: "POST",
        url: url,
        dataType: tipoRespuesta,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datos),
        success: function (data) {
            if (fSuceso) {
                fSuceso(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
            desbloquearPantalla();
            console.log("error del post: ------------------------");
            console.log(jqXHR);
            if (jqXHR.status == 403) {
                alert("Su sesión acaba de expirar. Por favor ingrese de nuevo su login");
                var respuestaRedireccion = JSON.parse(jqXHR.responseText);
                location.href = respuestaRedireccion.urlLogin;
            } else if (jqXHR.status == 401) {
                abrirModalLogin();
                //return;
            }

            if (fError) {
                fError(jqXHR, textStatus, errorThrown);
            }

        },
        timeout: timeoutDefecto
    });
    return xhr;
}

function manejarJsonBadRequest(jqXHR, idHtml) {
    if (jqXHR.status == 400) {
        var respuesta = JSON.parse(jqXHR.responseText);
        var html = "";

        for (i = 0; i < respuesta.mensajes.length; i++) {
            html += respuesta.mensajes[i] + '<br/>';
        }

        $('#' + idHtml + ' .mensajes').html(html);
        $('#' + idHtml).show();
    }
}

function bloquearPantalla(mensaje) {
}

function desbloquearPantalla() {
}

function obtenerStatus(jqXHR) {
    return jqXHR.status;
}

//--------------------- LOGIN MODAL ----------------------
function abrirModalLogin() {
    $("#loginUsuarioGeneralModal").modal("show");
    $("#tbUsuarioSesion").val("");
    $("#tbDniSesion").val("");
    $("#tbPasswordSesion").val("");
   
}

function ingresarLoginModal() {
    $("#errorContenedorSesionModal").hide();
    $("#btnIngresarSesion").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnIngresarSesion").prop("disabled", true);

    var datos = {
        username: $("#tbUsuarioSesion").val(),
        password: $("#tbPasswordSesion").val(),
        dni: $("#tbDniSesion").val()
    }
    realizarJsonPost($('#frmLoginSesionModal').attr('action'), datos, loginSucesoSesionModal, loginSesionModalError)

}

function loginSucesoSesionModal(resultado) {
    $("#btnIngresarSesion").html('Ingresar');
    $("#btnIngresarSesion").prop("disabled", false);    
    $("#loginUsuarioGeneralModal").modal("hide");

    var id = $("#hd_mostrar_alert_login_modal").val();
    if (id == "1") {
        MensajeAlerta("Su login se realizó correctamente, ahora puede continuar con su actividad", "success");
    }
}
function loginSesionModalError(jqXHR, textStatus, errorThrown) {
    $("#btnIngresarSesion").html('Ingresar');
    $("#btnIngresarSesion").prop("disabled", false);
    manejarJsonBadRequest(jqXHR, 'errorContenedorSesionModal');
}




//******************************************** VALIDACION DE NUMEROS **********************************
function SoloNumeros1_9() {
    var key = window.event ? event.which : event.keyCode;
    if (key < 48 || key > 57) {
        event.preventDefault();
    }
}

function SoloLetrasNumeros() {
    var key = window.event ? event.which : event.keyCode;
    if ((key < 65 || key > 90) && (key < 97 || key > 122)) {
        if (key < 48 || key > 57) {
            event.preventDefault();
        }
    }
}

function BorrarDatosSegunCodigo(s) {
    if (event.which == 8 || event.which == 46) {
        document.getElementById(s).value = "";
    }
}

function ValidarEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email) ? true : false;
}

function delayTime(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

//******************************************** CONSTANTES **********************************

const HTTP_CODIGOS = {
    OK: 200,
    BAD_REQUEST: 400,
    NOT_FOUND: 404,
    INTERNAL_SERVER_ERROR: 500
}

Object.freeze(HTTP_CODIGOS);

//***************************************** VALIDAR GRILLA *************************************//
function validarError(xhr) {

    if (xhr.status == 401) {
        location.reload();
        return;
    }

    if (xhr.status === 403) {
        var datoError = $.parseJSON(xhr.responseText);
        location.href = datoError.urlLogin;
    } else {
        alert('Ocurrio un error, contactar con soporte');
    }
}

function validarErrorGrilla(xhr, textStatus, error) {
    validarError(xhr);
}

/***************************MENSAJES ALERTA*********************************/
function MensajeAlerta(mensaje, tipo) {
    Swal.fire({
        text: mensaje,
        icon: tipo,
        buttonsStyling: true,
        confirmButtonText: "Cerrar",
        customClass: {
            confirmButton: "btn font-weight-bold btn-light"
        }
    });
}


function MensajeAlertaRedireccion(mensaje, tipo, redireccion) {
    Swal.fire({
        text: mensaje,
        icon: tipo,
        buttonsStyling: true,
        confirmButtonText: "Cerrar",
        customClass: {
            confirmButton: "btn font-weight-bold btn-light"
        }
    }).then(okay => {
            if (okay) {
                window.location.href = redireccion;
            }
    
    });
}


function MensajeCondicionalUno(titulo, detalle, fun, uno) {
    Swal.fire({
        title: titulo,
        text: detalle,
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "Sí"
    }).then(function (result) {
        if (result.value) {
            fun(uno);
        }
    });
}

function llenarCombo(url, datos, idCombo) {
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datos),
        success: function (data) {
            $('#' + idCombo)
                .find('option')
                .remove()
                .end();

            for (i = 0; i < data.length; i++) {
                var option = data[i];
                $('#' + idCombo)
                    .append('<option value="' + option.valor + '">' + option.texto + '</option>');
            }
            desbloquearPantalla();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            desbloquearPantalla();
            console.log(textStatus + ': ' + errorThrown);
            if (fError) {
                fError(jqXHR, textStatus, errorThrown);
            }

        }
    });
}



function compartirPopupWhatsapp(url) {
    $("#modalMensajeWhatsApp").modal("show");
    $("#txtMensajeWhatsappHtml").hide();
    $("#txtMensajeWhatsapp").val("Hola te invito a visualizar el detalle completo en el siguiente enlace.  " + url);

}

function enviarMensajeWhatsApp() {
    if ($("#txtMensajeWhatsapp").val().length > 0) {
        window.open(
            "https://api.whatsapp.com/send?text=" + encodeURIComponent($("#txtMensajeWhatsapp").val()),
            '_blank'
        );
    } else {
        $("#txtMensajeWhatsapp").focus();
        $("#txtMensajeWhatsappHtml").html("Mensaje es requerido");
        $("#txtMensajeWhatsappHtml").show();
    }
}

function cadenaAFecha(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

function numeroConComas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function redondearExp(numero, digitos) {
    function toExp(numero, digitos) {
        let arr = numero.toString().split("e");
        let mantisa = arr[0], exponente = digitos;
        if (arr[1]) exponente = Number(arr[1]) + digitos;
        return Number(mantisa + "e" + exponente.toString());
    }
    let entero = Math.round(toExp(Math.abs(numero), digitos));
    return Math.sign(numero) * toExp(entero, -digitos);
}

function formatoFecha(date) {
    var fecha = date.split("/");
    var dd = fecha[0];
    var mm = fecha[1];
    var yy = fecha[2];
    return yy + "-" + zeroFill(mm, 2) + "-" + zeroFill(dd, 2);
    //return texto.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$3/$2/$1');
}

function zeroFill(number, width) {
    width -= number.toString().length;
    if (width > 0) {
        return new Array(width + (/\./.test(number) ? 2 : 1)).join('0') + number;
    }
    return number + ""; // always return a string
}

$('.only-number').on('input', function () {
    var valor = $(this).val();
    valor = valor.replace(/[^\d.]/g, '');
    var partes = valor.split('.');
    if (partes.length > 2) {
        valor = partes[0] + '.' + partes.slice(1).join('');
    }
    $(this).val(valor);
});