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
    var dato = {
        tipo: $("#tbTipo").val(),
        idLote: $("#tbLote").val(),
    };
    realizarPost(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#contenidoCarta").show();
    $("#tbMes").html(data.mes);
    $("#tbAnio").html(data.anio);
    parametros = data;
    cargarGNSGNA(data.gnaGns);
}

function cargarGNSGNA(data) {
    var tbody = $("#gnsGnaTable tbody");
    tbody.empty(); // Limpiar cualquier fila existente

    data.forEach(function (item) {
        var c6 = item.c6 != null ? item.c6 : "";
        var c3 = item.c6 != null ? item.c3 : "";
        var ic4 = item.ic4 != null ? item.ic4 : "";
        var nc4 = item.nc4 != null ? item.nc4 : "";
        var neoC5 = item.neoC5 != null ? item.neoC5 : "";
        var ic5 = item.ic5 != null ? item.ic5 : "";
        var nc5 = item.nc5 != null ? item.nc5 : "";
        var nitrog = item.nitrog != null ? item.nitrog : "";
        var c1 = item.c1 != null ? item.c1 : "";
        var co2 = item.co2 != null ? item.co2 : "";
        var c2 = item.c2 != null ? item.c2 : "";
        var o2 = item.o2 != null ? item.o2 : "";
        var total = item.total != null ? item.total : "";
        var grav = item.grav != null ? item.grav : "";
        var btu = item.btu != null ? item.btu : "";
        var lgn = item.lgn != null ? item.lgn : "";
        var lgnrpte = item.lgnrpte != null ? item.lgnrpte : "";
        var row = `<tr>
        <td>${item.day}</td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${c6}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${c3}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${ic4}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${nc4}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${neoC5}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${ic5}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${nc5}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${nitrog}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${c1}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${co2}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${c2}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${o2}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${total}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${grav}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${btu}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${lgn}" /></td>
        <td><input type="text" class="form-control form-report only-number text-right small" value="${lgnrpte}" /></td>
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
    parametros.gnaGns = data;
    parametros.idLote = $("#tbLote").val();
    parametros.tipo = $("#tbTipo").val();;

    console.log(parametros);

    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}
function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}
function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
}