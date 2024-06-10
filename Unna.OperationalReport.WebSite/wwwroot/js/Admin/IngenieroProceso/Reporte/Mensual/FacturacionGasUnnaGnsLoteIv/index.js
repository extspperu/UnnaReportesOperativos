

$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnGuardar').click(function () {
        Obtener();
    });

}

function Obtener() {
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, GuardarError, 10000);
}

function RespuestaObtener(data) {
    Guardar(data);
}

function Guardar(parametros) {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();   
    parametros.mpc = $("#tbMpc").val();
    parametros.mmbtu = $("#tbMmbtu").val();
    parametros.precioUs = $("#tbPrecioUs").val();
    parametros.importeUs = $("#tbImporteUs").val();
    parametros.totalMpc = $("#tbTotalMpc").val();
    parametros.totalMmbtu = $("#tbTotalMmbtu").val();
    parametros.totalPrecioUs = $("#tbTotalPrecioUs").val();
    parametros.totalImporteUs = $("#tbTotalImporteUs").val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);

}