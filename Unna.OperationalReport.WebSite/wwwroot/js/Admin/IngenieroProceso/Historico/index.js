$(document).ready(function () {
    controles();
});

function controles() {
    $('#btn-3_GNA').click(() => descargarReporte('3_GNA'));
    $('#btn-1b_Tks').click(() => descargarReporte('1b_Tks'));
    $('#btn-3_Gas').click(() => descargarReporte('3_Gas'));
    $('#btn-Existencias').click(() => descargarReporte('Existencias'));
    $('#btn-2_Líquidos').click(() => descargarReporte('2_Líquidos'));
    Obtener();
}

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    console.log("Intentando ingresar al url ", url);
    var dato = {};
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    // Aquí puedes procesar los datos obtenidos si es necesario
}

function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}

function descargarReporte(tipoReporte) {
    var urlBase = $('#__URL_OBTENER').val();
    var url = `${urlBase}?tipoReporte=${encodeURIComponent(tipoReporte)}`;
    fetch(url)
        .then(response => response.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = `${tipoReporte}.xlsx`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        })
        .catch(error => console.error('Error al generar el reporte:', error));
}
