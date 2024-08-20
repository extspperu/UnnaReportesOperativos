var parametros;
$(document).ready(function () {
    controles();
})

function controles() {
    Obtener();
    $('#btnGuardar').click(function () {
        Guardar();
    });
}

function Obtener() {
    var url = $('#__URL_OBTENER_REPORTE').val();
    console.log("Intentando ingresar al url ", url);
    var dato = {};
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    console.log(data.glp);
    $("#contenidoCarta").show();
    parametros = data;
    cargarGLP(data.glp);
}

function cargarGLP(data) {
    var tbody = $("#glpTable tbody");
    tbody.empty(); 

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
function Guardar() {
    console.log("Iniciando");

    var url = $('#__URL_GUARDAR_REPORTE').val();
    var data = [];
    $('#glpTable tbody tr').each(function () {
        var row = $(this);
        var item = {
            day: parseInt(row.find('td:eq(0)').text(), 10),
            c1: parseFloat(row.find('td:eq(1) input').val()) || null,
            c2: parseFloat(row.find('td:eq(2) input').val()) || null,
            c3: parseFloat(row.find('td:eq(3) input').val()) || null,
            ic4: parseFloat(row.find('td:eq(4) input').val()) || null,
            nc4: parseFloat(row.find('td:eq(5) input').val()) || null,
            neoC5: parseFloat(row.find('td:eq(6) input').val()) || null,
            ic5: parseFloat(row.find('td:eq(7) input').val()) || null,
            nc5: parseFloat(row.find('td:eq(8) input').val()) || null,
            c6plus: parseFloat(row.find('td:eq(9) input').val()) || null,
            dRel: parseFloat(row.find('td:eq(10) input').val()) || null,
            presionVapor: parseFloat(row.find('td:eq(11) input').val()) || null,
            t95: parseFloat(row.find('td:eq(12) input').val()) || null,
            porcentajeMolarTotal: parseFloat(row.find('td:eq(13) input').val()) || null,
            tk: parseFloat(row.find('td:eq(14) input').val()) || null,
            despachos: parseFloat(row.find('td:eq(15) input').val()) || null
        };
        data.push(item);
    });

    var parametros = {
        glp: data,
        idLote: 0,
        tipo: "glp"
    };

    console.log("parametros ", parametros);

    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}


function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}
function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
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