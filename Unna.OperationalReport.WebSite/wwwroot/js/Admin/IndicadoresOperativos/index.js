$(document).ready(function () {
    controles();
});
function controles() {
    $('#ddlPeriodo').change(function () {
        BuscarIndicadores();
    });

    Periodos();
}

function Periodos() {
    var url = $("#__URL_OBTENER_PERIODOS").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaPeriodos, PeriodosError, 10000);
}

function RespuestaPeriodos(data) {
    console.log(data);

    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<option value="' + data[i].periodo + '">' + data[i].nombre + '</option>';
    }
    $("#ddlPeriodo").html(html);
    BuscarIndicadores();
}
function PeriodosError(data) {
    console.log(data);
}


function BuscarIndicadores() {
    var url = $("#__URL_BUSCAR_REGISTROS").val() + $("#ddlPeriodo").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarIndicadores, BuscarIndicadoresError, 10000);
}

function RespuestaBuscarIndicadores(data) {
    console.log(data);
    LlenarIndicadoresOperativos(data);

}
function BuscarIndicadoresError(data) {
    console.log(data);
}




var tablaIndicadores = null;
function LlenarIndicadoresOperativos(data) {
    if (tablaIndicadores) {
        tablaIndicadores.destroy();
        tablaIndicadores = null;
    }
    var table = $('#tblIndicadores').DataTable();
    table.destroy();

    var html = "";
    for (i = 0; i < data.length; i++) {
        if (data[i].idCategoriaPadre == null) {
            html += '<tr>';
            html += "<td>" + data[i].dia + "</td>";
            html += "<td>" + data[i].gna + "</td>";
            html += "<td>" + data[i].eficiencia + "</td>";
            html += "<td>" + data[i].glp + "</td>";
            html += "<td>" + data[i].lgn + "</td>";
            html += "</tr>";
        }

    }
    $("#tblIndicadores tbody").html(html);
    tablaIndicadores = $('#tblIndicadores').DataTable({
        "searching": false,
        "bLengthChange": false,        
        "info": true,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        },
        "aoColumnDefs": [
            {
                'bSortable': false,
                'aTargets': [0]
            }
        ],
    });
}