$(document).ready(function () {
    controles();
});

var destinatarios = [];
var cc = [];

function controles() {
    $('#selectReporte').change(function () {
        ObtenerDatosReporte();
    });
    $('#btnDescargarPdf').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Pdf/" + $("#selectReporte").val();
    });
    $('#btnDescargarExcel').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Excel/" + $("#selectReporte").val();
    });

    $('#btnAgregarDestinatario').click(function () {
        agregarDestinataro();
    });
    $('#tbDestinatario').keypress(function (e) {
        var keycode = e.keyCode || e.which;
        if (keycode == 13) {
            agregarDestinataro();
        }
    });
    $('#btnAgregarCc').click(function () {
        agregarCc();
    });
    
    $('#tbCc').keypress(function (e) {
        var keycode = e.keyCode || e.which;
        if (keycode == 13) {
            agregarCc();
        }
    });
    $('#btnEnviar').click(function () {
        Enviar();
    });
}


function ListarReportes() {
    var url = $('#__URL_LISTAR_REPORTES').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaListarReportes, ErrorListarReportes, 10000);
}

function RespuestaListarReportes(data) {
    for (var i = 0; i < data.length; i++) {
        $("#selectReporte").append('<option value="' + data[i].id + '">' + data[i].nombreReporte + '</option>');
    }
}

function ErrorListarReportes(data) {
    console.log(data);
}


function ObtenerDatosReporte() {
    $("#loadingContenidoCorreo").show();
    $("#contenidoCorreo").hide();
    limpiarContenido();
    var url = $('#__URL_DATOS_REPORTE').val() + $('#selectReporte').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtenerDatosReporte, ErrorObtenerDatosReporte, 10000);
}

function RespuestaObtenerDatosReporte(data) {
    $("#loadingContenidoCorreo").hide();
    $("#contenidoCorreo").show();
    if (data.fueEnviado) {
        $("#contenidoErrorBuscarReporte").show()
    } else {
        $("#contenidoSuccesBuscarReporte").show()
    }

    $("#mensajeErrorBuscarReporte").html(data.mensajeAlert)
    $("#mensajeSuccesBuscarReporte").html(data.mensajeAlert)
    if (data.tieneArchivoExcel) {
        $("#btnDescargarExcel").show()
    }
    if (data.tieneArchivoPdf) {
        $("#btnDescargarPdf").show()
    }

    if (data.destinatario != null) {
        destinatarios = data.destinatario;
        pintarCorreos(destinatarios);
    }
    if (data.cc != null) {
        cc = data.cc;
        pintarCorreosCc(cc);
    }


}
function ErrorObtenerDatosReporte(data) {
    $("#loadingContenidoCorreo").hide();
    console.log(data);
}

function pintarCorreos(correos) {
    var html = "";
    for (var i = 0; i < correos.length; i++) {
        html += '<li class="select2-selection__choice"><span class="select2-selection__choice__remove" onclick="quitarDestinatario(\'' + correos[i] + '\')">×</span>' + correos[i] + '</li>';
    }
    $(".select-correos-destinatario").html(html);
}

function quitarDestinatario(correo) {
    const resultado = destinatarios.filter(e => e != correo);
    destinatarios = resultado;
    pintarCorreos(destinatarios);
}
function agregarDestinataro() {
    if ($("#tbDestinatario").val().length > 0) {
        if (ValidarEmail($("#tbDestinatario").val())) {
            destinatarios.push($("#tbDestinatario").val().trim());
            $("#tbDestinatario").val("");
            $("#tbDestinatario").focus();
            pintarCorreos(destinatarios);
        }
    }
}


function pintarCorreosCc(correos) {
    var html = "";
    for (var i = 0; i < correos.length; i++) {
        html += '<li class="select2-selection__choice"><span class="select2-selection__choice__remove" onclick="quitarCc(\'' + correos[i] + '\')">×</span>' + correos[i] + '</li>';
    }
    $(".select-correos-cc").html(html);
}

function quitarCc(correo) {
    const resultado = cc.filter(e => e != correo);
    cc = resultado;
    pintarCorreosCc(cc);
}
function agregarCc() {
    if ($("#tbCc").val().length > 0) {
        if (ValidarEmail($("#tbCc").val())) {
            cc.push($("#tbCc").val().trim());
            $("#tbCc").val("");
            $("#tbCc").focus();
            pintarCorreosCc(cc);
        }
    }
}



function limpiarContenido() {
    $("#btnDescargarExcel").hide();
    $("#btnDescargarPdf").hide();
    $("#tbDestinatario").val("");
    $("#tbCc").val("");
    $("#tbAsunto").val("");
    $("#tbCuerpoCorreo").val("");
    $("#contenidoErrorBuscarReporte").hide();
    $("#contenidoSuccesBuscarReporte").hide();
}


function validarEnviar() {
    var flat = false;
    if (destinatarios.length == 0) {
        MensajeAlerta("Debe agregar destinatarios", "info");
        flat = true;
    } else if ($('#tbAsunto').val().length == 0) {
        MensajeAlerta("Asunto es requerido", "info");
        flat = true;
    } else if ($('#tbCuerpoCorreo').val().length == 0) {
        MensajeAlerta("Contenido del correo es requerido", "info");
        flat = true;
    }
    return flat;
}

function Enviar() {
    if (!validarEnviar()) {
        $("#btnEnviar").html('<i class="fa fa-spinner fa-spin"></i> Enviando...');
        $("#btnEnviar").prop("disabled", true);
        var url = $('#__URL_ENVIAR_REPORTE').val();
        var dato = {
            idReporte: $("#selectReporte").val(),
            asunto: $("#tbAsunto").val(),
            Cuerpo: $("#tbCuerpoCorreo").val(),
            destinatario: destinatarios,
            cc: cc,
        };
        realizarPost(url, dato, 'json', RespuestaEnviar, ErrorEnviar, 10000);
    }
}

function RespuestaEnviar(data) {
    console.log(data);
    MensajeAlerta("Se envió correctamente", "success");
    $("#btnEnviar").html('Enviar');
    $("#btnEnviar").prop("disabled", false);
    limpiarContenido();
}
function ErrorEnviar(data) {
    $("#btnEnviar").html('Enviar');
    $("#btnEnviar").prop("disabled", false);    
    var msg = data.responseJSON.mensajes[0];
    MensajeAlerta(msg, "error");
}
