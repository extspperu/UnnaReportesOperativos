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
    var url = $("#__URL_BUSCAR_REGISTROS").val() + $("#__URL_BUSCAR_REGISTROS").val() + $("#ddlPeriodo").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarIndicadores, BuscarIndicadoresError, 10000);
}

function RespuestaBuscarIndicadores(data) {
    console.log(data);

}
function BuscarIndicadoresError(data) {
    console.log(data);
}