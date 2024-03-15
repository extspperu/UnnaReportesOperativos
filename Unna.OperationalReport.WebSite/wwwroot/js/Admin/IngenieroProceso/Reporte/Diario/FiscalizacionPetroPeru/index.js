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
        Guardar();
    });
    Obtener();
}

function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}


function Obtener() {
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log("dato: ", data);
    parametros = data;
}

function ObtenerError(data) {
    console.log(data);
}



function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();
    parametros.volumenTotalProduccion = $("#volumenTotalProduccion").val();
    parametros.contenidoLgn = $("#contenidoLgn").val();
    parametros.eficiencia = $("#eficiencia").val();

    $('.list-datos-tablaFALGN').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factorAsignacionLiquidoGasNatural.length; i++) {
            if (parametros.factorAsignacionLiquidoGasNatural[i].item == datoId || parametros.factorAsignacionLiquidoGasNatural[i].item == null) {
                parametros.factorAsignacionLiquidoGasNatural[i].volumen = $("#FALGNVolumen_" + datoId).val().length > 0 ? $("#FALGNVolumen_" + datoId).val() : null;
                parametros.factorAsignacionLiquidoGasNatural[i].riqueza = $("#FALGNRiqueza_" + datoId).val().length > 0 ? $("#FALGNRiqueza_" + datoId).val() : null;
                parametros.factorAsignacionLiquidoGasNatural[i].contenido = $("#FALGNContenido_" + datoId).val().length > 0 ? $("#FALGNContenido_" + datoId).val() : null;
                parametros.factorAsignacionLiquidoGasNatural[i].factor = $("#FALGNFactor_" + datoId).val().length > 0 ? $("#FALGNFactor_" + datoId).val() : null;
                parametros.factorAsignacionLiquidoGasNatural[i].asignacion = $("#FALGNAsignacion_" + datoId).val().length > 0 ? $("#FALGNAsignacion_" + datoId).val() : null;
            }
        }
    });

    parametros.factorConversionZ69 = $("#FactorConversionZ69").val();
    parametros.factorConversionVi = $("#FactorConversionVi").val();
    parametros.factorConversionI = $("#FactorConversionI").val();
    $('.list-datos-tablaDGNS').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.distribucionGasNaturalSeco.length; i++) {
            if (parametros.distribucionGasNaturalSeco[i].item == datoId || parametros.distribucionGasNaturalSeco[i].item == null) {
                parametros.distribucionGasNaturalSeco[i].volumenGna = $("#DGNSVolumenGna_" + datoId).val().length > 0 ? $("#DGNSVolumenGna_" + datoId).val() : null;
                parametros.distribucionGasNaturalSeco[i].poderCalorifico = $("#DGNSPoderCalorifico_" + datoId).val().length > 0 ? $("#DGNSPoderCalorifico_" + datoId).val() : null;
                parametros.distribucionGasNaturalSeco[i].volumenGns = $("#DGNSVolumenGns_" + datoId).val().length > 0 ? $("#DGNSVolumenGns_" + datoId).val() : null;
                parametros.distribucionGasNaturalSeco[i].volumenGnsd = $("#DGNSVolumenGnsd_" + datoId).val().length > 0 ? $("#DGNSVolumenGnsd_" + datoId).val() : null;
            }
        }
    });

    parametros.volumenTotalGns = $("#VolumenTotalGns").val();
    $('.list-datos-tablaVTRPL').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenTransferidoRefineriaPorLote.length; i++) {
            if (parametros.volumenTransferidoRefineriaPorLote[i].item == datoId || parametros.volumenTransferidoRefineriaPorLote[i].item == null) {
                parametros.volumenTransferidoRefineriaPorLote[i].volumenGns = $("#VTRPLVolumenGns_" + datoId).val().length > 0 ? $("#VTRPLVolumenGns_" + datoId).val() : null;
                parametros.volumenTransferidoRefineriaPorLote[i].volumenFlare = $("#VTRPLVolumenFlare_" + datoId).val().length > 0 ? $("#VTRPLVolumenFlare_" + datoId).val() : null;
                parametros.volumenTransferidoRefineriaPorLote[i].volumenGnsTransferido = $("#VTRPLVolumenGnsTransferido_" + datoId).val().length > 0 ? $("#VTRPLVolumenGnsTransferido_" + datoId).val() : null;
            }
        }
    });

    parametros.volumenTotalGnsFlare = $("#VolumenTotalGnsFlare").val();

    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}