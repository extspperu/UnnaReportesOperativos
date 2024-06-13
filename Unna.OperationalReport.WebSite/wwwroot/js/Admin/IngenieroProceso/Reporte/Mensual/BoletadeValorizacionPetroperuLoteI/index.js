var parametros;

$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnDescargarPdf').click(function () {
        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
    //Obtener();
}

//function descargarExcel() {
//    window.location = $("#__HD_URL_GENERAR_REPORTE").val();
//}
function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
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
}

function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        //var datoId = datoIdSinSlashes.replace(/\//g, '');
        for (var i = 0; i < parametros.boletadeValorizacionPetroperuLoteIDet.length; i++) {
            if (parametros.boletadeValorizacionPetroperuLoteIDet[i].dia == datoId) {
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteIGNAMPCSD = $("#tbGasNaturalLoteIGNAMPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteIGNAMPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteIPCBTUPCSD = $("#tbGasNaturalLoteIPCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteIPCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteIEnergiaMMBTU = $("#tbGasNaturalLoteIEnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteIEnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteIRiquezaGALMPC = $("#tbGasNaturalLoteIRiquezaGALMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteIRiquezaGALMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteIRiquezaBLMMPC = $("#tbGasNaturalLoteIRiquezaBLMMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteIRiquezaBLMMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalLoteILGNRecupBBL = $("#tbGasNaturalLoteILGNRecupBBL_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteILGNRecupBBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasNaturalEficienciaPGT_Porcentaje = $("#tbGasNaturalEficienciaPGT_Porcentaje_" + datoId).val().length > 0 ? $("#tbGasNaturalEficienciaPGT_Porcentaje_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasSecoMS9215GNSLoteIMCSD = $("#tbGasSecoMS9215GNSLoteIMCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215GNSLoteIMCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasSecoMS9215PCBTUPCSD = $("#tbGasSecoMS9215PCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215PCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].gasSecoMS9215EnergiaMMBTU = $("#tbGasSecoMS9215EnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215EnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].precioGLPESinIGVSolesKG = $("#tbPrecioGLPESinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioGLPESinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].precioGLPGSinIGVSolesKG = $("#tbPrecioGLPGSinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioGLPGSinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].precioRefGLPSinIGVSolesKG = $("#tbPrecioRefGLPSinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioRefGLPSinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].precioGLPSinIGVUSBL = $("#tbPrecioGLPSinIGVUSBL_" + datoId).val().length > 0 ? $("#tbPrecioGLPSinIGVUSBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].tipodeCambioSoles_US = $("#tbTipodeCambioSoles_US_" + datoId).val().length > 0 ? $("#tbTipodeCambioSoles_US_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].precioCGNUSBL = $("#tbPrecioCGNUSBL_" + datoId).val().length > 0 ? $("#tbPrecioCGNUSBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].valorLiquidosUS = $("#tbValorLiquidosUS_" + datoId).val().length > 0 ? $("#tbValorLiquidosUS_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].costoUnitMaquilaUSMMBTU = $("#tbCostoUnitMaquilaUSMMBTU_" + datoId).val().length > 0 ? $("#tbCostoUnitMaquilaUSMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuLoteIDet[i].costoMaquilaUS = $("#tbCostoMaquilaUS_" + datoId).val().length > 0 ? $("#tbCostoMaquilaUS_" + datoId).val() : null;
            }

        }
    });
    parametros.fecha = $("#fecha").val();
    parametros.totalGasNaturalLoteIGNAMPCSD = $("#totalGasNaturalLoteIGNAMPCSD").val();
    parametros.totalGasNaturalLoteIEnergiaMMBTU = $("#totalGasNaturalLoteIEnergiaMMBTU").val();
    parametros.totalGasNaturalLoteILGNRecupBBL = $("#totalGasNaturalLoteILGNRecupBBL").val();
    parametros.totalGasNaturalEficienciaPGT = $("#totalGasNaturalEficienciaPGT").val();

    parametros.totalGasSecoMS9215GNSLoteIMCSD = $("#totalGasSecoMS9215GNSLoteIMCSD").val();
    parametros.totalGasSecoMS9215EnergiaMMBTU = $("#totalGasSecoMS9215EnergiaMMBTU").val();
    parametros.totalValorLiquidosUS = $("#totalValorLiquidosUS").val();
    parametros.totalCostoUnitMaquilaUSMMBTU = $("#totalCostoUnitMaquilaUSMMBTU").val();
    parametros.totalCostoMaquilaUS = $("#totalCostoMaquilaUS").val();

    parametros.totalDensidadGLPPromMesAnt = $("#totalDensidadGLPPromMesAnt").val();
    parametros.totalMontoFacturarporUnnaE = $("#totalMontoFacturarporUnnaE").val();
    parametros.totalMontoFacturarporPetroperu = $("#totalMontoFacturarporPetroperu").val();
    parametros.observacion1 = $("#observacion1").val();
    //parametros.observacion2 = $("#observacion2").val();
    //parametros.observacion3 = $("#observacion3").val();
    //parametros.observacion4 = $("#observacion4").val();

    console.log('Envio Post');
    console.log(parametros);
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);

}