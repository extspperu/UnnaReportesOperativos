$(document).ready(function () {
    controles();
});

var destinatarios = [];
var cc = [];

function controles() {
    $('.campo-fecha').datepicker({ format: "dd/mm/yyyy" });
    $('.campo-fecha').datepicker().on('changeDate', function (ev) { $('.campo-fecha').datepicker('hide'); });

    $('#selectReporte, #ddlGrupo, #ddlFecha').change(function () {
        BuscarCorreosEnviados();
    });
    $('#btnDescargarPdf').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Pdf/" + $("#cbxReporte").val();
    });
    $('#btnDescargarExcel').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Excel/" + $("#cbxReporte").val();
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
    $('#btnBuscar').click(function () {
        BuscarCorreosEnviados();
    });
    BuscarCorreosEnviados();
}
function BuscarCorreosEnviados() {
    var url = $('#__URL_LISTAR_CORREOS_ENVIADOS').val();
    var dato = {
        diaOperativo: generarFechaOrdenado($("#ddlFecha").val()),
        grupo: $("#ddlGrupo").val().length > 0 ? $("#ddlGrupo").val() : null,
        idReporte: $("#selectReporte").val().length > 0 ? $("#selectReporte").val() : null,
    };
    realizarPost(url, dato, 'json', RespuestaBuscarCorreosEnviados, ErrorBuscarCorreosEnviados, 10000);
}

function RespuestaBuscarCorreosEnviados(data) {
    console.log(data);
    LlenarTablasCorreosEnviados(data);
}

function ErrorBuscarCorreosEnviados(data) {
    console.log(data);
}

var tablaCorreosEnviados = null;
function LlenarTablasCorreosEnviados(data) {
    if (tablaCorreosEnviados) {
        tablaCorreosEnviados.destroy();
        tablaCorreosEnviados = null;
    }
    var table = $('#tblCorreos').DataTable();
    table.destroy();

    var html = "";
    for (i = 0; i < data.length; i++) {

        html += '<tr>';
        html += "<td>" + '<button onclick="ObtenerDatosReporte(\'' + data[i].idReporte + '\')" class="btn btn-sm btn-clean btn-icon mr-1" title="Enviar Correo">\
									<i class="flaticon-mail"></i>\
								</button></td>';
        html += "<td>" + data[i].nombreReporte + "</td>";
        html += "<td>" + data[i].grupo + "</td>";
        html += "<td>" + data[i].diaOperativo + "</td>";
        var fueEnviado = "";
        if (data[i].fueEnviado != null) {
            fueEnviado = data[i].fueEnviado ? "<span class='label label-success label-pill label-inline mr-2'>Si</span>" : "<span class='label label-danger label-pill label-inline mr-2'>No</span>";
        }
        html += "<td>" + fueEnviado + "</td>";

        var fechaEnvioCadena = "";
        if (data[i].fechaEnvioCadena != null) {
            fechaEnvioCadena = data[i].fechaEnvioCadena;
        }
        html += "<td>" + fechaEnvioCadena + "</td>";

        html += "</tr>";


    }
    $("#tblCorreos tbody").html(html);
    tablaCorreosEnviados = $('#tblCorreos').DataTable({
        "searching": false,
        "bLengthChange": false,
        "info": true,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        },
        "aoColumnDefs": [
            {
                'bSortable': false,
                'aTargets': [0]
            }
        ],
    });
}



function generarFechaOrdenado(fecha) {
    const [day2, month2, year2] = fecha.split('/');
    const hasta = [year2, month2, day2].join('-');
    return hasta;
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


function ObtenerDatosReporte(idReporte) {
    $("#loadingContenidoCorreo").show();
    $("#contenidoCorreo").hide();
    limpiarContenido();
    var url = $('#__URL_DATOS_REPORTE').val() + idReporte + "/" + generarFechaOrdenado($("#ddlFecha").val());
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtenerDatosReporte, ErrorObtenerDatosReporte, 10000);
}

function RespuestaObtenerDatosReporte(data) {
    $("#modalEnviarCorreo").modal("show");

    $("#loadingContenidoCorreo").hide();
    $("#contenidoCorreo").show();

    $("#tbAsunto").val(data.asunto);
    $("#tbCuerpoCorreo").val(data.cuerpo);
    $("#tbmDiaOperativo").val(data.diaOperativo);
    $("#cbxReporte").html('<option value="' + data.idReporte + '" selected>' + data.nombreReporte + '</option>');

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
            idReporte: $("#cbxReporte").val(),
            asunto: $("#tbAsunto").val(),
            Cuerpo: $("#tbCuerpoCorreo").val(),
            destinatario: destinatarios,
            cc: cc,
            diaOperativo: $("#tbmDiaOperativo").val()
        };
        realizarPost(url, dato, 'json', RespuestaEnviar, ErrorEnviar, 10000);
    }
}

function RespuestaEnviar(data) {
    console.log(data);
    MensajeAlerta("Se envió correctamente", "success");
    $("#btnEnviar").html('Enviar');
    $("#btnEnviar").prop("disabled", false);
    $("#modalEnviarCorreo").modal("hide");
    limpiarContenido();
    BuscarCorreosEnviados();
}

function ErrorEnviar(data) {
    $("#btnEnviar").html('Enviar');
    $("#btnEnviar").prop("disabled", false);
    var msg = data.responseJSON.mensajes[0];
    MensajeAlerta(msg, "error");
    BuscarCorreosEnviados();
}
