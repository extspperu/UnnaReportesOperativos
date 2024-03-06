var ListaDocumentos = [];
var cargaReporte;

$(document).ready(function () {
    controles();
});
function controles() {
    $('#btnCargarReporte').change(function () {
        CargarReporte();
    });
    $('#btnAdjuntarDocumento').click(function () {
        AdjuntarDocumentoModal();
    });
    $('#btnAgregarComentario').click(function () {
        AdjuntarComentario();
    });
    $('#btnSubirDocumento').change(function () {
        InsertarDocumento();
    });
    $('#txtComentario').keypress(function () {
        $("#txtComentarioHtml").hide();
    });
    $('#btnGuardar').click(function () {
        Guardar();
    });
    $('#btnGuardarComentario').click(function () {
        GuardarCompletario();
    });
    $('#btnGuardarAdjuntos').click(function () {
        GuardarAdjuntos();
    });
    $('input:checkbox[name="checkConciliado"]').click(function () {        
        if ($(this).is(':checked')) {
            $("#estadoColor_" + $(this).val()).html('<div class="campo-estado verde"></div>');
        } else {
            $("#estadoColor_" + $(this).val()).html('<div class="campo-estado rojo"></div>');
        }
    });
    Obtener();
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

function InsertarDocumento() {
    $("#btnGuardarDocumento").prop("disabled", true);
    $("#btnSubirDocumentoLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_DOCUMENTOS').val();
    var dato = new FormData($("#FormDocumentos")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnSubirDocumentoLabel").html("Adjuntar Documentos");
            $("#btnGuardarDocumento").prop("disabled", false);
            document.getElementById("btnSubirDocumento").value = null;            
            if (ListaDocumentos.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var existe = ListaDocumentos.filter(e => e.idAdjunto == data[i].idAdjunto);
                    if (existe.length == 0) {
                        ListaDocumentos.push(data[i]);
                    } else {
                        ListaDocumentos[i].idAdjunto = data[i].idAdjunto;
                        ListaDocumentos[i].color = data[i].color;
                        ListaDocumentos[i].esConciliado = data[i].esConciliado;
                        ListaDocumentos[i].idArchivo = data[i].idArchivo;
                        ListaDocumentos[i].adjunto = data[i].adjunto;
                        ListaDocumentos[i].archivo = data[i].archivo;
                    }
                }
            } else {
                ListaDocumentos = data;
            }

            RefrescarTablaDocumentos();
        },
        error: function (jqXHR, status, error) {
            console.log(jqXHR);
            $("#btnSubirDocumentoLabel").html("Adjuntar Documentos");
            document.getElementById("btnSubirDocumento").value = null;
            $("#btnGuardarDocumento").prop("disabled", false);
            var mensaje = jqXHR.responseJSON.mensajes[0];
            MensajeAlerta(mensaje, "info");
        },
    });
}




function ValidarCamposRequeridoCategoria() {
    var flat = true;
    if ($("#txtNombre").val().length === 0) {
        $("#txtNombre").focus();
        $("#txtNombreHtml").html("Nombre es requerido, ingrese por favor.");
        $("#txtNombreHtml").show();
        flat = false;
    } else if ($("#txtUrl").val().length === 0) {
        $("#txtUrl").focus();
        $("#txtUrlHtml").html("Url es requerido, ingrese por favor.");
        $("#txtUrlHtml").show();
        flat = false;
    }
    return flat;
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

function eliminarAdjunto(id) {
    var existe = ListaDocumentos.filter(x => x.idAdjunto == id);
    var nombreArchivo = "";
    if (existe.length > 0) {
        if (existe[0].adjunto != null) {
            nombreArchivo = existe[0].adjunto.nomenclatura + existe[0].adjunto.extension;
        }
        
    }
    var index = ListaDocumentos.findIndex(x => x.idAdjunto == id);
    if (index > -1) {
        ListaDocumentos.splice(index, 1);
    }    
    $("#tbNombreArchivo_" + id).html('<h4 class="nombre-archivo">' + nombreArchivo+'</h4>')
    $('#checkConciliado_' + id).prop('checked', false);
    $('#checkConciliado_' + id).prop("disabled", false);
    $("#estadoColor_" + id).html('<div class="campo-estado rojo"></div>');
    RefrescarTablaDocumentos();
}

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#txtComentario").val(data.comentario);
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


function CargarReporte() {
    $("#btnCargarReporteLabel").prop("disabled", true);
    $("#btnCargarReporteLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_DOCUMENTO').val();
    var dato = new FormData($("#FormCargarReporte")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnCargarReporteLabel").html("Adjuntar Documentos");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
            cargaReporte = data;
            refrescarVisorReporte();
        },
        error: function (jqXHR, status, error) {
            $("#btnCargarReporteLabel").html("Adjuntar Documentos");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
        },
    });
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
