
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

    parametros.tabla1.gasMpcd = $("#Tabla1GasMpcd").val();
    parametros.tabla1.glpBls = $("#Tabla1GlpBls").val();
    parametros.tabla1.cgnBls = $("#Tabla1CgnBls").val();
    parametros.tabla1.cnsMpc = $("#Tabla1CnsMpc").val();
    parametros.tabla1.cgMpc = $("#Tabla1CgMpc").val();

    parametros.volumenTotalGnsEnMs = $("#VolumenTotalGnsEnMs").val();
    parametros.volumenTotalGns = $("#VolumenTotalGns").val();
    parametros.flareGna = $("#FlareGna").val();

    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factoresDistribucionGasNaturalSeco.length; i++) {
            if (parametros.factoresDistribucionGasNaturalSeco[i].item == datoId || parametros.factoresDistribucionGasNaturalSeco[i].item == null) {
                parametros.factoresDistribucionGasNaturalSeco[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].concentracionC1 = $("#ConcentracionC1_" + datoId).val().length > 0 ? $("#ConcentracionC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].volumenC1 = $("#VolumenC1_" + datoId).val().length > 0 ? $("#VolumenC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].factoresDistribucion = $("#FactoresDistribucion_" + datoId).val().length > 0 ? $("#FactoresDistribucion_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].asignacionGns = $("#AsignacionGns_" + datoId).val().length > 0 ? $("#AsignacionGns_" + datoId).val() : null;
            }
        }
    });

    $('.list-datos-tablaFDGDC').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factoresDistribucionGasDeCombustible.length; i++) {
            if (parametros.factoresDistribucionGasDeCombustible[i].item == datoId || parametros.factoresDistribucionGasDeCombustible[i].item == null) {
                parametros.factoresDistribucionGasDeCombustible[i].volumen = $("#FDGDCVolumen_" + datoId).val().length > 0 ? $("#FDGDCVolumen_" + datoId).val() : null;
                parametros.factoresDistribucionGasDeCombustible[i].concentracionC1 = $("#FDGDCConcentracionC1_" + datoId).val().length > 0 ? $("#FDGDCConcentracionC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasDeCombustible[i].volumenC1 = $("#FDGDCVolumenC1_" + datoId).val().length > 0 ? $("#FDGDCVolumenC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasDeCombustible[i].factoresDistribucion = $("#FDGDCFactoresDistribucion_" + datoId).val().length > 0 ? $("#FDGDCFactoresDistribucion_" + datoId).val() : null;
                parametros.factoresDistribucionGasDeCombustible[i].asignacionGns = $("#FDGDCAsignacionGns_" + datoId).val().length > 0 ? $("#FDGDCAsignacionGns_" + datoId).val() : null;
            }
        }
    });

    parametros.volumenProduccionTotalGlp = $("#VolumenProduccionTotalGlp").val();
    parametros.volumenProduccionTotalCgn = $("#VolumenProduccionTotalCgn").val();
    parametros.volumenProduccionTotalLgn = $("#VolumenProduccionTotalLgn").val();
    $('.list-datos-tablaFDLGN').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factoresDistribucionLiquidoGasNatural.length; i++) {
            if (parametros.factoresDistribucionLiquidoGasNatural[i].item == datoId || parametros.factoresDistribucionLiquidoGasNatural[i].item == null) {
                parametros.factoresDistribucionLiquidoGasNatural[i].volumen = $("#FDLGNVolumen_" + datoId).val().length > 0 ? $("#FDLGNVolumen_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].riqueza = $("#FDLGNRiqueza_" + datoId).val().length > 0 ? $("#FDLGNRiqueza_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].contenido = $("#FDLGNContenido_" + datoId).val().length > 0 ? $("#FDLGNContenido_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].factoresDistribucion = $("#FDLGNFactoresDistribucion_" + datoId).val().length > 0 ? $("#FDLGNFactoresDistribucion_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].asignacionGns = $("#FDLGNAsignacionGns_" + datoId).val().length > 0 ? $("#FDLGNAsignacionGns_" + datoId).val() : null;
            }
        }
    });

    parametros.volumenTotalGasCombustible = $("#VolumenTotalGasCombustible").val();

    parametros.gravedadEspecifica = $("#GravedadEspecifica").val();
    parametros.volumenProduccionTotalGlpCnpc = $("#VolumenProduccionTotalGlpCnpc").val();
    parametros.volumenProduccionTotalCgnCnpc = $("#VolumenProduccionTotalCgnCnpc").val();
    console.log("parametros ", parametros);
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}