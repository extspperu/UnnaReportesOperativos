$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
}

function descargarExcel() {
    window.location = $("#__HD_URL_GENERAR_REPORTE").val();
}