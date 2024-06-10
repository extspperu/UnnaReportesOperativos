
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
    console.log(parametros);
    if (!parametros.factoresAsignacionGasCombustible) {
        console.error("FactoresAsignacionGasCombustible no está definido en los datos recibidos.");
    }
}

function ObtenerError(data) {
    console.log(data);
}

function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();


    // PRIMER CUADRO
    parametros.VolumenTotalGasCombustible = $("#VolumenTotalGasCombustible").val();

    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        console.log("datoid ", datoId);
        for (var i = 0; i < parametros.factoresAsignacionGasCombustible.length; i++) {
            console.log(parametros.factoresAsignacionGasCombustible[i].item);
            if (parametros.factoresAsignacionGasCombustible[i].item == datoId || parametros.factoresAsignacionGasCombustible[i].item == null) {
                parametros.factoresAsignacionGasCombustible[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.factoresAsignacionGasCombustible[i].calorifico = $("#Calorifico_" + datoId).val().length > 0 ? $("#Calorifico_" + datoId).val() : null;
                parametros.factoresAsignacionGasCombustible[i].energiaMmbtu = $("#EnergiaMmbtu_" + datoId).val().length > 0 ? $("#EnergiaMmbtu_" + datoId).val() : null;
                parametros.factoresAsignacionGasCombustible[i].factorAsignacion = $("#FactorAsignacion_" + datoId).val().length > 0 ? $("#FactorAsignacion_" + datoId).val() : null;
                parametros.factoresAsignacionGasCombustible[i].asignacion = $("#Asignacion_" + datoId).val().length > 0 ? $("#Asignacion_" + datoId).val() : null;
            }
        }
    });

    // SEGUNDO CUADRO
    parametros.VolumenTotalGns = $("#VolumenTotalGns").val();
    $('.list-datos-tablaFactorAsignacionGns').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factorAsignacionGns.length; i++) {
            if (parametros.factorAsignacionGns[i].item == datoId || parametros.factorAsignacionGns[i].item == null) {
                parametros.factorAsignacionGns[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.factorAsignacionGns[i].calorifico = $("#Calorifico_" + datoId).val().length > 0 ? $("#Calorifico_" + datoId).val() : null;
                parametros.factorAsignacionGns[i].energiaMmbtu = $("#EnergiaMmbtu_" + datoId).val().length > 0 ? $("#EnergiaMmbtu_" + datoId).val() : null;
                parametros.factorAsignacionGns[i].factorAsignacion = $("#FactorAsignacion_" + datoId).val().length > 0 ? $("#FactorAsignacion_" + datoId).val() : null;
                parametros.factorAsignacionGns[i].asignacion = $("#Asignacion_" + datoId).val().length > 0 ? $("#Asignacion_" + datoId).val() : null;
            }
        }
    });

    // TERCER CUADRO
    parametros.VolumenProduccionTotalGlp = $("#VolumenProduccionTotalGlp").val();
    parametros.VolumenProduccionTotalCgn = $("#VolumenProduccionTotalCgn").val();
    parametros.VolumenProduccionTotalLgn = $("#VolumenProduccionTotalLgn").val();
    $('.list-datos-tablaFALGN').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factorAsignacionLiquidosGasNatural.length; i++) {
            if (parametros.factorAsignacionLiquidosGasNatural[i].item == datoId || parametros.factorAsignacionLiquidosGasNatural[i].item == null) {
                parametros.factorAsignacionLiquidosGasNatural[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.factorAsignacionLiquidosGasNatural[i].riqueza = $("#Riqueza_" + datoId).val().length > 0 ? $("#Riqueza_" + datoId).val() : null;
                parametros.factorAsignacionLiquidosGasNatural[i].contenido = $("#Contenido_" + datoId).val().length > 0 ? $("#Contenido_" + datoId).val() : null;
                parametros.factorAsignacionLiquidosGasNatural[i].factorAsignacion = $("#FactorAsignacion_" + datoId).val().length > 0 ? $("#FactorAsignacion_" + datoId).val() : null;
                parametros.factorAsignacionLiquidosGasNatural[i].asignacion = $("#Asignacion_" + datoId).val().length > 0 ? $("#Asignacion_" + datoId).val() : null;
            }
        }
    });

    parametros.VolumenProduccionTotalGlpLoteIv = $("#VolumenProduccionTotalGlpLoteIv").val();
    parametros.VolumenProduccionTotalCgnLoteIv = $("#VolumenProduccionTotalCgnLoteIv").val();
    parametros.FactorCoversion = $("#FactorCoversion").val();

    // CUARTO CUADRO

    parametros.distribucionGasNaturalAsociado.volumenGnsd = $("#DGNAVolumenGnsd").val();
    parametros.distribucionGasNaturalAsociado.GasCombustible = $("#DGNAGasCombustible").val();
    parametros.distribucionGasNaturalAsociado.volumenGns = $("#DGNAVolumenGns").val();
    parametros.distribucionGasNaturalAsociado.volumenGna = $("#DGNAVolumenGna").val();

    parametros.VolumenGnsVentaVgnsvTotal = $("#VolumenGnsVentaVgnsvTotal").val();
    parametros.VolumenGnsVentaVgnsvEnel = $("#VolumenGnsVentaVgnsvEnel").val();
    parametros.VolumenGnsVentaVgnsvGasnorp = $("#VolumenGnsVentaVgnsvGasnorp").val();
    parametros.VolumenGnsVentaVgnsvLimagas = $("#VolumenGnsVentaVgnsvLimagas").val();
    parametros.VolumenGnsFlareVgnsrf = $("#VolumenGnsFlareVgnsrf").val();

    parametros.SumaVolumenGasCombustibleVolumen = $("#SumaVolumenGasCombustibleVolumen").val();
    parametros.VolumenGnaFiscalizado = $("#VolumenGnaFiscalizado").val();
    console.log(parametros);
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
}