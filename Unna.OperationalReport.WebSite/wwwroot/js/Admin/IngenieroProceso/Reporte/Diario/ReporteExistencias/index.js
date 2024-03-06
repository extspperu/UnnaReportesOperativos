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
    window.location = $("#__URL_GENERAR_REPORTE").val();
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
    var datos = parametros.datos;
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
       
        if ($("#txtCapacidadInstalada_" + datoId).val().length === 0) {
            MensajeAlerta("Debe ingresar la capacidad instalada", "error");
            return;
        }
        if ($("#txtExistenciaDiaria_" + datoId).val().length === 0) {
            MensajeAlerta("Debe ingresar existencia diaria", "error");
            return;
        }
        for (var i = 0; i < datos.length; i++) {
            if (datos[i].item === datoId) {
                datos[i].capacidadInstalada = $("#txtCapacidadInstalada_" + datoId).val();
                datos[i].existenciaDiaria = $("#txtExistenciaDiaria_" + datoId).val();
            }
        }
    });

    var url = $('#__URL_GUARDAR_REPORTE').val();
    parametros.datos = datos;
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("No se pudo completar el registro", "error");

}