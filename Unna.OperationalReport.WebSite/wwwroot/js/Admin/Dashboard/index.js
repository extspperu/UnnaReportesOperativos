$(document).ready(function () {
    controles();
});

function controles() {
    obtenerDatosPowerBI();
}

function obtenerDatosPowerBI() {
    var url = $('#__URL_OBTENER').val();
    console.log("Intentando obtener datos de PowerBI desde la URL: ", url);
    var dato = {};
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

//function realizarGet(url, data, dataType, successCallback, errorCallback, timeout) {
//    $.ajax({
//        type: 'GET',
//        url: url,
//        data: data,
//        dataType: dataType,
//        success: successCallback,
//        error: errorCallback,
//        timeout: timeout
//    });
//}
function RespuestaObtener(data) {
    console.log(data);
    // Aquí puedes procesar los datos obtenidos si es necesario
}
function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}
function respuestaObtenerDatosPowerBI(data) {
    console.log("Datos obtenidos de PowerBI:", data);
    // Usar los datos obtenidos para incrustar el informe de PowerBI
    var embedConfiguration = {
        type: 'report',
        id: data.ReportId,
        embedUrl: data.EmbedUrl,
        accessToken: data.AccessToken,
        tokenType: window['powerbi-client'].models.TokenType.Aad,
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: true
        }
    };

    var reportContainer = document.getElementById('reportContainer');
    var powerbi = new window['powerbi-client'].service.Service();
    powerbi.embed(reportContainer, embedConfiguration);
}

function errorObtenerDatosPowerBI(data) {
    console.error("Error al obtener datos de PowerBI:", data);
    $("#contenidoErrorMensaje").show();
}
