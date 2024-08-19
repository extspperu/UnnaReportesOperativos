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
    console.log(data);
    $("#tbAnio").html(data.anio);
    $("#tbTipo").html(data.tipo);
    $("#tbTanque").html(data.tanque);
    $("#tbMes").html(data.mes);
    cargarCGN(data.cgn);
}

function cargarCGN(data) {
    var tbody = $("#cgnTable tbody");
    tbody.empty(); 

    data.forEach(function (item) {
        var row = `<tr>
            <td>${item.day}</td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.pinic}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p5}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p10}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p30}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p50}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p70}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p90}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.p95}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.pfin}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.api}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.gesp}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.rvp}" /></td>
            <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nroDespacho}" /></td>
        </tr>`;
        tbody.append(row);
    });
}

function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}


function Guardar() {
    console.log("Iniciando");

    var url = $('#__URL_GUARDAR_REPORTE').val();
    var data = [];
    $('#cgnTable tbody tr').each(function () {
        var row = $(this);
        var item = {
            day: parseInt(row.find('td:eq(0)').text(), 10),
            pinic: parseFloat(row.find('td:eq(1) input').val()) || null,
            p5: parseFloat(row.find('td:eq(2) input').val()) || null,
            p10: parseFloat(row.find('td:eq(3) input').val()) || null,
            p30: parseFloat(row.find('td:eq(4) input').val()) || null,
            p50: parseFloat(row.find('td:eq(5) input').val()) || null,
            p70: parseFloat(row.find('td:eq(6) input').val()) || null,
            p90: parseFloat(row.find('td:eq(7) input').val()) || null,
            p95: parseFloat(row.find('td:eq(8) input').val()) || null,
            pfin: parseFloat(row.find('td:eq(9) input').val()) || null,
            api: parseFloat(row.find('td:eq(10) input').val()) || null,
            gesp: parseFloat(row.find('td:eq(11) input').val()) || null,
            rvp: parseFloat(row.find('td:eq(12) input').val()) || null,
            nroDespacho: parseFloat(row.find('td:eq(13) input').val()) || null
        };
        data.push(item);
    });

    var parametros = {
        cgn: data,
        idLote: $("#tbLote").val(),
        tipo: $("#tbTipo").val()
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