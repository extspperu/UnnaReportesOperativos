//$(document).ready(function () {
//    controles();
//});

//function controles() {
//    obtenerDatosPowerBI();
//}

//function obtenerDatosPowerBI() {
//    var url = $('#__URL_OBTENER').val();
//    console.log("Intentando obtener datos de PowerBI desde la URL: ", url);
//    var dato = {};
//    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
//}

//function RespuestaObtener(data) {
//    console.log(data);
//RespuestaObtener
//function ErrorObtener(data) {
//    console.log(data);
//    $("#contenidoErrorMensaje").show();
//}
//function respuestaObtenerDatosPowerBI(data) {
//    console.log("Datos obtenidos de PowerBI:", data);
//    var embedConfiguration = {
//        type: 'report',
//        id: data.ReportId,
//        embedUrl: data.EmbedUrl,
//        accessToken: data.AccessToken,
//        tokenType: window['powerbi-client'].models.TokenType.Aad,
//        settings: {
//            filterPaneEnabled: false,
//            navContentPaneEnabled: true
//        }
//    };

//    var reportContainer = document.getElementById('reportContainer');
//    var powerbi = new window['powerbi-client'].service.Service();
//    powerbi.embed(reportContainer, embedConfiguration);
//}

//function errorObtenerDatosPowerBI(data) {
//    console.error("Error al obtener datos de PowerBI:", data);
//    $("#contenidoErrorMensaje").show();
//}

$(document).ready(function () {
    embedPowerBIReport();
});

function embedPowerBIReport() {
    console.log("Iniciando...");
    var embedToken = $('#__EMBED_TOKEN').val();
    var embedUrl = $('#__EMBED_URL').val();
    var reportId = $('#__REPORT_ID').val();
    var embedConfiguration = {
        type: 'report',
        id: reportId,
        embedUrl: embedUrl,
        accessToken: embedToken,
        tokenType: window['powerbi-client'].models.TokenType.Aad,
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: true
        }
    };

    var reportContainer = document.getElementById('reportContainer');
    var powerbi = new window['powerbi-client'].service.Service();
    powerbi.embed(reportContainer, embedConfiguration);
    console.log(powerbi);
    console.log(embedConfiguration);
}
