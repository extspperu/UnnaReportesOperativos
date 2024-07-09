var ListaDocumentos = [];
var cargaReporte;

$(document).ready(function () {
    controles();
});
function controles() {
    $('#btnSubirFirma').change(function () {
        SubirFirma();
    });
    $('#btnSubirFdddirma').click(function () {
        AdjuntarDocumentoModal();
    });


    Obtener();
}

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    realizarGet(url, {}, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#tbDocumento").val(data.documento);
    $("#tbNombres").val(data.nombres);
    $("#tbPaterno").val(data.paterno);
    $("#tbMaterno").val(data.materno);
    $("#tbTelefono").val(data.telefono);
    $("#tbCorreo").val(data.username);
    if (data.urlFirma != null) {
        $('#__URL_IMAGEN_FIRMA').show();
        $('#__URL_IMAGEN_FIRMA').attr("src", data.urlFirma);
    }
    
    
}
function ObtenerError(data) {
    console.log(data);
}



function SubirFirma() {
    $("#btnSubirFirmaLabel").prop("disabled", true);
    $("#btnSubirFirmaLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_DOCUMENTO').val();
    var dato = new FormData($("#FormFirma")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnSubirFirmaLabel").html("Subir Firma");
            $("#btnSubirFirmaLabel").prop("disabled", false);
            document.getElementById("btnSubirFirma").value = null;
            $('#__URL_IMAGEN_FIRMA').show();
            $('#__URL_IMAGEN_FIRMA').attr("src", data.url);
            ActualizarFirma(data.id);
        },
        error: function (jqXHR, status, error) {
            console.log(jqXHR);
            $("#btnSubirFirmaLabel").html("Adjuntar Documentos");
            $("#btnSubirFirmaLabel").prop("disabled", false);
            document.getElementById("btnSubirFirma").value = null;            
            var mensaje = jqXHR.responseJSON.mensajes[0];
            MensajeAlerta(mensaje, "info");
        },
    });
}

function ActualizarFirma(id) {
    var url = $('#__URL_ACTUALIZAR_FIRMA').val() + id;
    realizarGet(url, {}, 'json', RespuestaActualizarFirma, ActualizarFirmaError, 10000);
}

function RespuestaActualizarFirma(data) {
    console.log(data);
    $("#tbDocumento").val(data.documento)
    $("#tbNombres").val(data.nombres)
    $("#tbPaterno").val(data.paterno)
    $("#tbMaterno").val(data.materno)
    $("#tbTelefono").val(data.telefono)
    $("#tbCorreo").val(data.username)

}
function ActualizarFirmaError(data) {
    console.log(data);
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
