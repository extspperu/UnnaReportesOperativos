var ListaDocumentos = [];

$(document).ready(function () {
    controles();
});
function controles() {

    $('#btnAdjuntarDocumento').click(function () {
        AdjuntarDocumento();
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
    Obtener();
}


function AdjuntarDocumento() {
    $("#agregarDocumentosModal").modal("show");
}

function AdjuntarComentario() {
    $("#agregarComentarioModal").modal("show");
}


function RefrescarTablaDocumentos() {
    var html = "";
    for (var i = 0; i < ListaDocumentos.length; i++) {
        html += '<p class="d-flex justify-content-between"><a href="' + ListaDocumentos[i].url + '" target="_blank" >' + ListaDocumentos[i].nombre + '</a> <a class="cerrar" href="javascript:void(0)" ';
        if ($("#__HD_ES_EDICION").val().length > 0) {
            html += 'onclick="eliminarAdjunto(\'' + ListaDocumentos[i].id + '\')"';
        }
        html += '>X</a></p>';
    }
    $("#archivosAdjuntos").html(html);
}

function InsertarDocumento() {
    $("#btnGuardarDocumento").prop("disabled", true);
    $("#btnSubirDocumentoLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_DOCUMENTO').val();
    var dato = new FormData($("#FormDocumentos")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnSubirDocumentoLabel").html("Adjuntar Documentos");
            document.getElementById("btnSubirDocumento").value = null;
            $("#btnGuardarDocumento").prop("disabled", false);
            ListaDocumentos.push(data);
            RefrescarTablaDocumentos();
        },
        error: function (jqXHR, status, error) {
            $("#btnSubirDocumentoLabel").html("Adjuntar Documentos");
            document.getElementById("btnSubirDocumento").value = null;
            $("#btnGuardarDocumento").prop("disabled", false);
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

function Guardar() {
    var url = $('#__URL_GUARDAR_REGISTRO_DIA').val();
    var datos = [];
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        datos.push({
            idDato: datoId,
            valor: $("#txtValorDato_" + datoId).val(),
            esConciliado: $('#checkConciliado_' + datoId).prop('checked'),
        });
    });
    var valores = datos.filter(e => e.valor === null || e.valor === '');    
    if (valores.length > 0) {
        MensajeAlerta("Debe ingresar todos los valores", "error");
        return;
    }

    var dato = {
        "adjuntos": JSON.stringify(ListaDocumentos),
        "comentario": $("#txtComentario").val(),
        "registros": datos,
        "idGrupo": $("#__HD_GRUPO").val()
    };
    realizarPost(url, dato, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#agregarDocumentosModal").modal("hide");
    $("#agregarComentarioModal").modal("hide");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

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
    if (ListaDocumentos.length > 0) {
        Guardar();
    } else {
        MensajeAlerta("Debe cargar por lo menos un documento para guardar", "error");
    }

}

function eliminarAdjunto(id) {
    var index = ListaDocumentos.findIndex(x => x.id == id);
    if (index > -1) {
        ListaDocumentos.splice(index, 1);
    }
    RefrescarTablaDocumentos();
}

function Obtener() {
    var url = $('#__URL_OBTENER_REGISTRO_DIA').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    if (data.adjuntos !== null) {
        ListaDocumentos = JSON.parse(data.adjuntos);
    }
    $("#txtComentario").val(data.comentario);
    if(data.registros.length > 0) {
        for (var i = 0; i < data.registros.length; i++) {
            $('#checkConciliado_' + data.registros[i].idDato).prop('checked', data.registros[i].esConciliado);
            $('#txtValorDato_' + data.registros[i].idDato).val(data.registros[i].valor);
        }
    }
    RefrescarTablaDocumentos();
}

function ObtenerError(data) {
    console.log(data);
}