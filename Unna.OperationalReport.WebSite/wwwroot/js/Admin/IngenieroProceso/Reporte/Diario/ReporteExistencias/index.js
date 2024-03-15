var parametros;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
}

function descargarExcel() {
    $("#btnDescargarExcel").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnDescargarExcel").prop("disabled", true);
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
    $("#btnDescargarExcel").html('Descargar');
    $("#btnDescargarExcel").prop("disabled", false);
}



function Obtener() {
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);

    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {

    console.log("dato: ", data);
    parametros = data;
    Guardar();
}

function ObtenerError(data) {
    console.log(data);
    MensajeAlerta("No se pudo completar el registro", "error");
}



function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    if (parametros.datos == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    if (parametros.datos.length === 0) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.datos.length; i++) {
            if (parametros.datos[i].item == datoId) {
                parametros.datos[i].capacidadInstalada = $("#txtCapacidadInstalada_" + datoId).val().length > 0 ? $("#txtCapacidadInstalada_" + datoId).val() : null;
                parametros.datos[i].existenciaDiaria = $("#txtExistenciaDiaria_" + datoId).val().length > 0 ? $("#txtExistenciaDiaria_" + datoId).val() : null;
            }
        }
    });

    var url = $('#__URL_GUARDAR_REPORTE').val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(jqXHR) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = jqXHR.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}