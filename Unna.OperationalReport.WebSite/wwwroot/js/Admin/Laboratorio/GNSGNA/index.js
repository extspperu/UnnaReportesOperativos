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
    var dato = { };
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    $("#contenidoCarta").show();
    parametros = data;
    cargarGNSGNA(data);
}

function cargarGNSGNA(data) {
    var tbody = $("#gnsGnaTable tbody");
    tbody.empty(); // Limpiar cualquier fila existente

    data.forEach(function(item) {
        var row = `<tr>
        <td>${item.day}</td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c6}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c3}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.ic4}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nc4}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.neoC5}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.ic5}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nc5}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.nitrog}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c1}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.co2}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.c2}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.o2}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.total}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.grav}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.btu}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.lgn}" /></td>
        <td><input type="number" class="form-control form-report only-number text-right small" value="${item.lgnrpte}" /></td>
        <td>
            <label class="switch">
                <input type="checkbox" class="form-control">
                    <span class="slider round"></span>
            </label>
        </td>
        <td><input type="text" class="form-control form-report text-right small" /></td>
    </tr>`;
    tbody.append(row);
    });
}

function ErrorObtener(data) {
    $("#contenidoErrorMensaje").show();
}

function Guardar() {
    console.log("Iniciando");

    var url = $('#__URL_GUARDAR_REPORTE').val();
    var data = [];
    $('#gnsGnaTable tbody tr').each(function () {
        var row = $(this);
        var item = {
            Day: parseInt(row.find('td:eq(0)').text(), 10),
            C6: parseFloat(row.find('td:eq(1) input').val()) || 0,
            C3: parseFloat(row.find('td:eq(2) input').val()) || 0,
            IC4: parseFloat(row.find('td:eq(3) input').val()) || 0,
            NC4: parseFloat(row.find('td:eq(4) input').val()) || 0,
            NeoC5: parseFloat(row.find('td:eq(5) input').val()) || 0,
            IC5: parseFloat(row.find('td:eq(6) input').val()) || 0,
            NC5: parseFloat(row.find('td:eq(7) input').val()) || 0,
            NITROG: parseFloat(row.find('td:eq(8) input').val()) || 0,
            C1: parseFloat(row.find('td:eq(9) input').val()) || 0,
            CO2: parseFloat(row.find('td:eq(10) input').val()) || 0,
            C2: parseFloat(row.find('td:eq(11) input').val()) || 0,
            O2: parseFloat(row.find('td:eq(12) input').val()) || 0,
            TOTAL: parseFloat(row.find('td:eq(13) input').val()) || 0,
            GRAV: parseFloat(row.find('td:eq(14) input').val()) || 0,
            BTU: parseFloat(row.find('td:eq(15) input').val()) || 0,
            LGN: parseFloat(row.find('td:eq(16) input').val()) || 0,
            LGNRPTE: parseFloat(row.find('td:eq(17) input').val()) || 0,
            Conciliado: row.find('td:eq(18) input').is(':checked') ? true : false,
            Comentarios: row.find('td:eq(19) input').text()
        };
        data.push(item);
    });
    var test = {
        IdUsuario: 1
        ,Data: data
    };

    console.log(test);

    realizarPost(url, test, 'json', RespuestaGuardar, GuardarError, 10000);
}
function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}
function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
}