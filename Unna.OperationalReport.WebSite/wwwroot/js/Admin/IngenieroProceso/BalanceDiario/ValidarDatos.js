var ListaDocumentos = [];
var registros = [];

$(document).ready(function () {
    controles();
});
function controles() {

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
    $('#btnGuardar, #btnDevolver').click(function () {
        Guardar();
    });
    $('#btnGuardarComentario').click(function () {
        GuardarCompletario();
    });
    
    $('#btnObservar').click(function () {
        $("#modalConfirmacion").modal("show");
    });

    $('.radio-validacion').click(function () {
        validarRadios();
    });
    $('#btnEditar').click(function () {
        $(".edit-text").prop("disabled", false);
        $("#btnObservar").hide();
        $("#btnGuardar").show();
        $('#__ACCION').val("EDITAR");
        $("#modalConfirmacion").modal("hide");
    });
    $('#btnGuardarEditar').click(function () {
        GuardarEditar();
    });
    Obtener();
}

function validarRadios() {
    $("#btnGuardar").hide();
    $("#btnObservar").hide();
    var tieneObservado = false;
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        var value = $('input:radio[name="radios_' + datoId + '"]:checked').val();
        if (value === false || value === 'false') {
            tieneObservado = true;
        }
    });
    if (tieneObservado) {
        $("#btnObservar").show();
    } else {
        $("#btnGuardar").show();
    }
}


function AdjuntarDocumentoModal() {
    RefrescarTablaDocumentos();
    $("#agregarDocumentosModal").modal("show");
}

function AdjuntarComentario() {
    $("#agregarComentarioModal").modal("show");
}


function RefrescarTablaDocumentos() {
    var html = "";
    for (var i = 0; i < ListaDocumentos.length; i++) {
        html += '<p class="d-flex justify-content-between"><a href="' + ListaDocumentos[i].url + '" target="_blank" >' + ListaDocumentos[i].nombre + '</a>';
        html += '</p>';
    }
    $("#archivosAdjuntos").html(html);
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
    var url = $('#__URL_GUARDAR').val();
    if ($('#__ACCION').val() === "EDITAR") {
        url = $('#__URL_GUARDAR_EDITAR').val();
    }
    for (var i = 0; i < registros.length; i++) {
        var datoId = registros[i].idDato;
        registros[i].valor = $("#txtValorDato_" + datoId).val();
        registros[i].esConciliado = $('#checkConciliado_' + datoId).prop('checked');
        var value = $('input:radio[name="radios_' + datoId + '"]:checked').val();
        if (value === false || value === 'false') {
            registros[i].esValido = false;
        }
        if (value === true || value === 'true') {
            registros[i].esValido = true;
        }
    }
    var validoRegistro = registros.filter(e => e.esValido === null);
    if (validoRegistro.length > 0) {
        MensajeAlerta("Para guardar debe validar todos los datos", "error");
        return;
    }
    realizarPost(url, registros, 'json', RespuestaGuardar, GuardarError, 10000);
}


function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#modalConfirmacion").modal("hide");
    if ($('#__ACCION').val() === "EDITAR") {
        $(".edit-text").prop("disabled", true);
        $("#btnObservar").hide();
        $("#btnGuardar").show();
        $('#__ACCION').val("");
    }    
}

function GuardarError(data) {
    var mensaje = data.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "error");

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


function Obtener() {
    var url = $('#__URL_OBTENER').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    if (data.adjuntos !== null) {
        ListaDocumentos = JSON.parse(data.adjuntos);
    }
    $("#txtComentario").val(data.comentario);
    registros = data.registros;
    if (data.registros.length > 0) {
        for (var i = 0; i < data.registros.length; i++) {
            $('#checkConciliado_' + data.registros[i].idDato).prop('checked', data.registros[i].esConciliado);
            $('#txtValorDato_' + data.registros[i].idDato).val(data.registros[i].valor);
            if (data.registros[i].esValido === false) {
                $('.validado_' + data.registros[i].idDato).find('input:radio[value="false"]').prop('checked', true);
            } else if (data.registros[i].esValido === true) {
                $('.validado_' + data.registros[i].idDato).find('input:radio[value="true"]').prop('checked', true);
            }
        }
        if (registros.filter(e => e.esValido == false).length > 0) {
            $("#btnGuardar").hide();
            $("#btnObservar").show();
        }
    }

    RefrescarTablaDocumentos();
}

function ObtenerError(data) {
    console.log(data);
}