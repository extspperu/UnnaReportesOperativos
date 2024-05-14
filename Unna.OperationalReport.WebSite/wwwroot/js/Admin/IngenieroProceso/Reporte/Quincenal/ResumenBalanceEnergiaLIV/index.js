
var parametros;
$(document).ready(function () {
    controles();

});


function controles() {
    $('#btnDescargarExcel').click(function () {
        console.log("Iniciando EXCEL");

        descargarExcel();
    });
    $('#btnDescargarPdf').click(function () {
        console.log("Iniciando PDF ");

        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });

    $('#btnDescargarLGNExcel').click(function () {
        console.log("Iniciando excel LGN");
        descargarLGNExcel();
    });
    $('#btnDescargarLGNPdf').click(function () {
        console.log("Iniciando PDF LGN");
        descargarLGNPdf();
    });

}

function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}

function descargarLGNExcel() {
    var url = $("#__URL_GENERAR_REPORTELGN_EXCEL").val();
    console.log(url); 
    window.location = url;
}
function descargarLGNPdf() {
    window.location = $("#__URL_GENERAR_REPORTELGN_PDF").val();
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
    Guardar();
}

function ObtenerError(data) {
    console.log(data);
}

function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();


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


    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}