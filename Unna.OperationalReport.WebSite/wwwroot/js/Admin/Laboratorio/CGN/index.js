var parametros;
$(document).ready(function () {
    controles();
})

function controles() {
    Obtener();
}

function Obtener() {
    var url = $('#__URL_OBTENER_REPORTE').val();
    console.log("Intentando ingresar al url ", url);
    var dato = {};
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#contenidoCarta").show();
    parametros = data;
    cargarCGN(data);
}

function cargarCGN(data) {
    var tbody = $("#cgnTable tbody");
    tbody.empty(); // Limpiar cualquier fila existente

    data.forEach(function (item) {
        var row = `<tr>
            <td>${item.day}</td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.pInicial}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor5}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor10}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor30}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor50}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor70}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor90}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.valor95}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.pFinal}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.api}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.gEsp}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.rvp}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nDespachos}" /></td>
        </tr>`;
        tbody.append(row);
    });
}

function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}