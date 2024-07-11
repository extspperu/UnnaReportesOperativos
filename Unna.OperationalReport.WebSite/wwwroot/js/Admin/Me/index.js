var ListaDocumentos = [];
var cargaReporte;

$(document).ready(function () {
    controles();
});
function controles() {
    $('#btnSubirFirma').change(function () {
        SubirFirma();
    });
    $('#btnGuardar').click(function () {
        Guardar();
    });


    Obtener();
}

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    realizarGet(url, {}, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
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
    MensajeAlerta("Se guardó su firma correctamente", "success");

}
function ActualizarFirmaError(data) {
    console.log(data);
    MensajeAlerta("No se completo la carga de firma, intente nuevamente", "error");
}





function Guardar() {
    if (!ValidarEmail($("#tbCorreo").val())) {
        $("#tbCorreo").focus();
        MensajeAlerta("Ingrese un correo válido", "info");
        return;
    }
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
    var url = $('#__URL_GUARDAR_INFORMACION').val();
    var data = {
        documento: $("#tbDocumento").val(),
        paterno: $("#tbPaterno").val(),
        materno: $("#tbMaterno").val(),
        nombres: $("#tbNombres").val(),
        correo: $("#tbCorreo").val(),
        telefono: $("#tbTelefono").val(),
    };
    realizarPost(url, data, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");

}

function GuardarError(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = data.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}
