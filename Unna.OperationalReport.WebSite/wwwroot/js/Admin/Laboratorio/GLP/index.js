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
    cargarGLP(data);
}

function cargarGLP(data) {
    var tbody = $("#glpTable tbody");
    tbody.empty(); // Limpiar cualquier fila existente

    data.forEach(function (item) {
        var row = `<tr>
            <td style="font-weight: bold;">${item.day}</td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c1}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c2}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c3}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.ic4}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nc4}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.neoC5}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.ic5}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nc5}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c6plus}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.dRel}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.presionVapor}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.t95}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.porcentajeMolarTotal}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.tk}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.despachos}" /></td>
            <td><button class="btn-add-row" onclick="addRow(this)">+</button></td>
        </tr>`;
        tbody.append(row);
    });
}

function addRow(button) {
    var row = button.closest('tr');
    var newRow = row.cloneNode(true);
    newRow.querySelector('.btn-add-row').remove();
    newRow.querySelector('td:first-child').style.fontWeight = 'normal';
    row.parentNode.insertBefore(newRow, row.nextSibling);
}

function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}