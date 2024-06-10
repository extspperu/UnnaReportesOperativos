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
        for (var i = 0; i < parametros.boletadeValorizacionPetroperuDet.length; i++) {
            if (parametros.boletadeValorizacionPetroperuDet[i].dia == datoId) {
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteIGNAMPCSD = $("#tbGasNaturalLoteVIGNAMPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIGNAMPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteIPCBTUPCSD = $("#tbGasNaturalLoteVIPCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIPCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteIEnergiaMMBTU = $("#tbGasNaturalLoteVIEnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIEnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteIRiquezaGALMPC = $("#tbGasNaturalLoteVIRiquezaGALMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIRiquezaGALMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteIRiquezaBLMMPC = $("#tbGasNaturalLoteVIRiquezaBLMMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIRiquezaBLMMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteILGNRecupBBL = $("#tbGasNaturalLoteVILGNRecupBBL_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVILGNRecupBBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVIGNAMPCSD = $("#tbGasNaturalLoteVIGNAMPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIGNAMPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVIPCBTUPCSD = $("#tbGasNaturalLoteVIPCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIPCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVIEnergiaMMBTU = $("#tbGasNaturalLoteVIEnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIEnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVIRiquezaGALMPC = $("#tbGasNaturalLoteVIRiquezaGALMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIRiquezaGALMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVIRiquezaBLMMPC = $("#tbGasNaturalLoteVIRiquezaBLMMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVIRiquezaBLMMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteVILGNRecupBBL = $("#tbGasNaturalLoteVILGNRecupBBL_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteVILGNRecupBBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69GNAMPCSD = $("#tbGasNaturalLoteZ69GNAMPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69GNAMPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69PCBTUPCSD = $("#tbGasNaturalLoteZ69PCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69PCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69EnergiaMMBTU = $("#tbGasNaturalLoteZ69EnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69EnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69RiquezaGALMPC = $("#tbGasNaturalLoteZ69RiquezaGALMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69RiquezaGALMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69RiquezaBLMMPC = $("#tbGasNaturalLoteZ69RiquezaBLMMPC_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69RiquezaBLMMPC_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLoteZ69LGNRecupBBL = $("#tbGasNaturalLoteZ69LGNRecupBBL_" + datoId).val().length > 0 ? $("#tbGasNaturalLoteZ69LGNRecupBBL_" + datoId).val() : null;

                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalTotalGNAMPCSD = $("#tbGasNaturalTotalGNAMPCSD_" + datoId).val().length > 0 ? $("#tbGasNaturalTotalGNAMPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalEficienciaPGT_Porcentaje = $("#tbGasNaturalEficienciaPGT_Porcentaje_" + datoId).val().length > 0 ? $("#tbGasNaturalEficienciaPGT_Porcentaje_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasNaturalLiquidosRecupTotalesBBL = $("#tbGasNaturalLiquidosRecupTotalesBBL_" + datoId).val().length > 0 ? $("#tbGasNaturalLiquidosRecupTotalesBBL_" + datoId).val() : null;

                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215GNSLoteIMCSD = $("#tbGasSecoMS9215GNSLoteIMCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215GNSLoteIMCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215GNSLoteVIMCSD = $("#tbGasSecoMS9215GNSLoteVIMCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215GNSLoteVIMCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215GNSLoteZ69MCSD = $("#tbGasSecoMS9215GNSLoteZ69MCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215GNSLoteZ69MCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215GNSTotalMCSD = $("#tbGasSecoMS9215GNSTotalMCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215GNSTotalMCSD_" + datoId).val() : null;

                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215PCBTUPCSD = $("#tbGasSecoMS9215PCBTUPCSD_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215PCBTUPCSD_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].gasSecoMS9215EnergiaMMBTU = $("#tbGasSecoMS9215EnergiaMMBTU_" + datoId).val().length > 0 ? $("#tbGasSecoMS9215EnergiaMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].precioGLPESinIGVSolesKG = $("#tbPrecioGLPESinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioGLPESinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].precioGLPGSinIGVSolesKG = $("#tbPrecioGLPGSinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioGLPGSinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].precioRefGLPSinIGVSolesKG = $("#tbPrecioRefGLPSinIGVSolesKG_" + datoId).val().length > 0 ? $("#tbPrecioRefGLPSinIGVSolesKG_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].precioGLPSinIGVUSBL = $("#tbPrecioGLPSinIGVUSBL_" + datoId).val().length > 0 ? $("#tbPrecioGLPSinIGVUSBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].tipodeCambioSoles_US = $("#tbTipodeCambioSoles_US_" + datoId).val().length > 0 ? $("#tbTipodeCambioSoles_US_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].precioCGNUSBL = $("#tbPrecioCGNUSBL_" + datoId).val().length > 0 ? $("#tbPrecioCGNUSBL_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].valorLiquidosUS = $("#tbValorLiquidosUS_" + datoId).val().length > 0 ? $("#tbValorLiquidosUS_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].costoUnitMaquilaUSMMBTU = $("#tbCostoUnitMaquilaUSMMBTU_" + datoId).val().length > 0 ? $("#tbCostoUnitMaquilaUSMMBTU_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperuDet[i].costoMaquilaUS = $("#tbCostoMaquilaUS_" + datoId).val().length > 0 ? $("#tbCostoMaquilaUS_" + datoId).val() : null;
            }

        }
    });
    parametros.fecha = $("#fecha").val();
    parametros.totalGasNaturalGNAMPCSD = $("#totalGasNaturalLoteIGNAMPCSD").val();
    parametros.totalGasNaturalEnergiaMMBTU = $("#totalGasNaturalLoteIEnergiaMMBTU").val();
    parametros.totalGasNaturalLGNRecupBBL = $("#totalGasNaturalLoteILGNRecupBBL").val();
    parametros.totalGasNaturalGNAMPCSD = $("#totalGasNaturalLoteVIGNAMPCSD").val();
    parametros.totalGasNaturalEnergiaMMBTU = $("#totalGasNaturalLoteVIEnergiaMMBTU").val();
    parametros.totalGasNaturalLGNRecupBBL = $("#totalGasNaturalLoteVILGNRecupBBL").val();
    parametros.totalGasNaturalGNAMPCSD = $("#totalGasNaturalLoteZ69GNAMPCSD").val();
    parametros.totalGasNaturalEnergiaMMBTU = $("#totalGasNaturalLoteZ69EnergiaMMBTU").val();
    parametros.totalGasNaturalLGNRecupBBL = $("#totalGasNaturalLoteZ69LGNRecupBBL").val();

    parametros.totalGasNaturalTotalGNA = $("#totalGasNaturalTotalGNA").val();
    parametros.totalGasNaturalEficienciaPGT = $("#totalGasNaturalEficienciaPGT").val();
    parametros.totalGasNaturalLiquidosRecupTotales = $("#totalGasNaturalLiquidosRecupTotales").val();

    parametros.totalGasSecoMS9215GNSLoteIMCSD = $("#totalGasSecoMS9215GNSLoteIMCSD").val();
    parametros.totalGasSecoMS9215GNSLoteVIMCSD = $("#totalGasSecoMS9215GNSLoteVIMCSD").val();
    parametros.totalGasSecoMS9215GNSLoteZ69MCSD = $("#totalGasSecoMS9215GNSLoteZ69MCSD").val();
    parametros.totalGasSecoMS9215GNSTotalMCSD = $("#totalGasSecoMS9215GNSTotalMCSD").val();

    parametros.totalGasSecoMS9215EnergiaMMBTU = $("#totalGasSecoMS9215EnergiaMMBTU").val();
    parametros.totalValorLiquidosUS = $("#totalValorLiquidosUS").val();
    parametros.totalCostoUnitMaquilaUSMMBTU = $("#totalCostoUnitMaquilaUSMMBTU").val();
    parametros.totalCostoMaquilaUS = $("#totalCostoMaquilaUS").val();

    parametros.totalDensidadGLPPromMesAnt = $("#totalDensidadGLPPromMesAnt").val();
    parametros.totalMontoFacturarporUnnaE = $("#totalMontoFacturarporUnnaE").val();
    parametros.totalMontoFacturarporPetroperu = $("#totalMontoFacturarporPetroperu").val();
    parametros.observacion1 = $("#observacion1").val();
    parametros.observacion2 = $("#observacion2").val();
    parametros.observacion3 = $("#observacion3").val();


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