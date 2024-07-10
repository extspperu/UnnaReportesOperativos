
var ListaDocumentos = [];
var cargaReporte;

$(document).ready(function () {
    controles();
});
function controles() {
    $('#btnCargarReporte').change(function () {
        CargarReporte();
    });

    $('#btnGuardar').click(function () {
        Guardar();
    });

    $('#btnAdjuntarDocumento').click(function () {
        $("#agregarDocumentosModal").modal("show");
    });
    $('#btnAgregarComentario').click(function () {
        $("#agregarComentarioModal").modal("show");
    });
    $('#btnObservar').click(function () {
        $("#modalConfirmacion").modal("show");
    });
    $('#btnDevolver').click(function () {
        ObservarRegistros();
    });
    $('#btnValidar').click(function () {
        ValidarRegistros();
    });


    Obtener();
}


function ObservarRegistros() {
    if ($("#__HD_ID_REGISTRO").val().length == 0) {
        MensajeAlerta("No se puede observar, porque aun no se ha cargado los registro para el dia operativo", "info");
        return;
    }
    $("#btnDevolver").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnDevolver").prop("disabled", true);
    var url = $('#__URL_OBSERVAR_REGISTRO').val() + $('#__HD_ID_REGISTRO').val();
    realizarGet(url, {}, 'json', RespuestaObservarRegistros, ObservarRegistroError, 10000);
}

function RespuestaObservarRegistros(data) {
    $("#btnDevolver").html('DEVOLVER');
    $("#btnDevolver").prop("disabled", false);
    MensajeAlerta("Se envió observación correctamente", "success");
    $("#modalConfirmacion").modal("hide");
    Obtener();
}
function ObservarRegistroError(data) {
    $("#btnDevolver").html('DEVOLVER');
    $("#btnDevolver").prop("disabled", false);    
    MensajeAlerta(data.responseJSON.mensajes[0], "error");
}

function ValidarRegistros() {
    if ($("#__HD_ID_REGISTRO").val().length == 0) {
        MensajeAlerta("No se puede validar, porque aun no se ha cargado los registro para el dia operativo", "info");
        return;
    }
    $("#btnValidar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnValidar").prop("disabled", true);
    var url = $('#__URL_VALIDAR_REGISTRO').val() + $('#__HD_ID_REGISTRO').val();
    realizarGet(url, {}, 'json', RespuestaValidarRegistros, ValidarRegistrosError, 10000);
}

function RespuestaValidarRegistros(data) {
    $("#btnValidar").html('VALIDAR');
    $("#btnValidar").prop("disabled", false);
    MensajeAlerta("Se validó correctamente", "success");
    Obtener();
}
function ValidarRegistrosError(data) {
    $("#btnValidar").html('VALIDAR');
    $("#btnValidar").prop("disabled", false);
    MensajeAlerta(data.responseJSON.mensajes[0], "error");
}



function AdjuntarDocumentoModal() {
    RefrescarTablaDocumentos();
    $("#agregarDocumentosModal").modal("show");
}

function AdjuntarComentario() {
    $("#agregarComentarioModal").modal("show");
}


function RefrescarTablaDocumentos() {
    var diabled = false;
    for (var i = 0; i < ListaDocumentos.length; i++) {
        if (ListaDocumentos[i].archivo != null) {
            if (ListaDocumentos[i].archivo.nombre != null) {
                var nombre = '<h4 class="nombre-archivo d-flex justify-content-between" style="color:#000;"><a href="' + ListaDocumentos[i].archivo.url + '">' + ListaDocumentos[i].archivo.nombre + '</a><a class="cerrar" href="javascript:void(0)" onclick="eliminarAdjunto(\'' + ListaDocumentos[i].idAdjunto + '\')">x</a></h4>'
                $("#tbNombreArchivo_" + ListaDocumentos[i].idAdjunto).html(nombre);
                diabled = true;
            }
        }
        $("#estadoColor_" + ListaDocumentos[i].idAdjunto).html('<div class="campo-estado verde"></div>');
        $('#checkConciliado_' + ListaDocumentos[i].idAdjunto).prop('checked', ListaDocumentos[i].esConciliado);
        $('#checkConciliado_' + ListaDocumentos[i].idAdjunto).prop("disabled", diabled);
    }

}





function ObtenerDatos() {

    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');

        if ($('#checkConciliado_' + datoId).prop('checked')) {
            var existe = ListaDocumentos.filter(e => e.idAdjunto == datoId);
            if (existe.length == 0) {
                ListaDocumentos.push({
                    idAdjunto: datoId,
                    esConciliado: $('#checkConciliado_' + datoId).prop('checked'),
                    archivo: null
                });
            } else {
                for (var i = 0; i < ListaDocumentos.length; i++) {
                    if (ListaDocumentos[i].idAdjunto == datoId) {
                        ListaDocumentos[i].esConciliado = $('#checkConciliado_' + datoId).prop('checked')
                    }
                }
            }
        }
    });
    var dato = {
        "adjuntos": ListaDocumentos,
        "comentario": $("#txtComentario").val(),
        "idArchivo": cargaReporte.id,
    };
    return dato;
}

function Guardar() {
    if (cargaReporte == null) {
        MensajeAlerta("Debe cargar reporte de excel", "error");
        return;
    }
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
    var url = $('#__URL_GUARDAR_REGISTRO').val();
    realizarPost(url, ObtenerDatos(), 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('GUARDAR');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
    $("#agregarDocumentosModal").modal("hide");
    $("#agregarComentarioModal").modal("hide");
}

function GuardarError(data) {
    $("#btnGuardar").html('GUARDAR');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = data.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}

function GuardarCompletario() {

    if ($("#txtComentario").val().length > 0) {
        Guardar();
    } else {
        $("#txtComentario").focus();
        $("#txtComentarioHtml").html("Comentario es requerido, ingrese por favor.");
        $("#txtComentarioHtml").show();
    }

}

function GuardarAdjuntos() {
    var url = $('#__URL_GUARDAR_REGISTRO_ADJUNTO').val();
    realizarPost(url, ObtenerDatos(), 'json', RespuestaGuardar, GuardarError, 10000);
}



function Obtener() {
    $("#mensajeValidadoHtml").hide();
    $("#mensajeObservadoHtml").hide();
    var url = $('#__URL_OBTENER').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log("data ", data);
    $("#__HD_ID_REGISTRO").val(data.id);
    $("#txtComentario").val(data.comentario);

    if (data.esValidado === true) {
        
        $("#mensajeValidadoHtml").show();
        $("#mensajeValidadoHtml").html('<div class="alert-text">Registro fue validado correctamente  <b>' + data.fechaValidado + '</b> </div> ');
    } else if (data.esObservado === true) {
        $("#mensajeObservadoHtml").show();
        $("#mensajeObservadoHtml").html('<div class="alert-text">Registro fue observado <b>' + data.fechaObservado + '</b></div>');
    }

    cargaReporte = data.archivo;
    refrescarVisorReporte();

    if (data.adjuntos != null) {
        ListaDocumentos = data.adjuntos;
    }
    RefrescarTablaDocumentos();
}

function ObtenerError(data) {
    console.log(data);
}


function refrescarVisorReporte() {
    var html = "";
    if (cargaReporte != null) {
        html = '<p class="d-flex justify-content-between" style="border: 0px solid #fff;font-size: 18px;font-weight: 600;margin-bottom: 0px;"><a href="' + cargaReporte.url + '" >' + cargaReporte.nombre + '</a> <a class="cerrar" href="javascript:void(0)" onclick="quitarReporteCargado(\'' + cargaReporte.id + '\')">x</a></p>';
    }
    $("#cargarReporteResultado").html(html);
}

function quitarReporteCargado() {
    cargaReporte = null;
    refrescarVisorReporte();
}
