
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
    window.location = $("#__HD_URL_GENERAR_REPORTE_EXCEL").val();
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
    Guardar(data);
}

function ObtenerError(data) {
    console.log(data);
}

function Guardar(parametros) {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.valores.length; i++) {
            if (parametros.valores[i].id == datoId) {
                parametros.valores[i].volumen = $("#tbVolumen_" + datoId).val().length > 0 ? $("#tbVolumen_" + datoId).val() : null;
                parametros.valores[i].poderCalorifico = $("#tbPoderCalorifico_" + datoId).val().length > 0 ? $("#tbPoderCalorifico_" + datoId).val() : null;
                parametros.valores[i].energia = $("#tbEnergia_" + datoId).val().length > 0 ? $("#tbEnergia_" + datoId).val() : null;                
            }

        }
    });
    parametros.totalVolumen = $("#txtTotalVolumen").val();
    parametros.totalPc = $("#txtTotalPc").val();
    parametros.totalEnergia = $("#txtTotalEnergia").val();
    parametros.energiaVolumenProcesado = $("#txtEnergiaVolumenProcesado").val();
    parametros.precioUsd = $("#txtPrecioUsd").val();
    parametros.subTotal = $("#txtSubTotal").val();
    parametros.igv = $("#txtIgv").val();
    parametros.totalFacturar = $("#txtTotalFacturar").val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);

}