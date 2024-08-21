$(document).ready(function () {
    controles();
});


function controles() {
    $('.campo-fecha').datepicker({ format: "dd/mm/yyyy" });
    $('.campo-fecha').datepicker().on('changeDate', function (ev) { $('.campo-fecha').datepicker('hide'); });

    $('#btnBuscar').click(function () {
        BuscarComponentes();
    });
    $('#ddlLotes, #ddlFecha').change(function () {
        BuscarComponentes();
    });

    $('#btnNuevo').click(function () {
        registrarNuevo();
    });
    $('#btnLimpiar').click(function () {
        Cancelar();

    });
    $('#btnGuardar').click(function () {
        Guardar();
    });

    ListarLotes();
}

function registrarNuevo() {
    $("#tblNuevoComponentes").show();
    $("#tblComponentes").hide();
    $("#ddlLotes").prop("disabled", true);
    $("#ddlFecha").prop("disabled", true);
    $("#btnNuevo").prop("disabled", true);
    $("#btnBuscar").prop("disabled", true);
    $("#btnGuardar").prop("disabled", false);
    BuscarNuevo();
}

function Cancelar() {
    $("#tblNuevoComponentes").hide();
    $("#tblComponentes").show();
    $("#ddlFecha").val($("#__HD_FECHA").val());
    $("#ddlLotes").prop("disabled", false);
    $("#ddlFecha").prop("disabled", false);
    $("#btnNuevo").prop("disabled", false);
    $("#btnBuscar").prop("disabled", false);
    $("#btnGuardar").prop("disabled", true);
    BuscarComponentes();
}

function ListarLotes() {
    var url = $("#__URL_LISTAR_LOTES").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaListarLotes, ListarLotesError, 10000);
}

function RespuestaListarLotes(data) {
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<option value="' + data[i].id + '">' + data[i].nombre + '</option>';
    }
    $("#ddlLotes").html(html);
    BuscarComponentes();
}
function ListarLotesError(data) {
    console.log(data);
}

function generarFechaOrdenado(fecha) {
    const [day2, month2, year2] = fecha.split('/');
    const hasta = [year2, month2, day2].join('-');
    return hasta;
}


function BuscarComponentes() {
    var url = $("#__URL_LISTAR_COMPONENTE").val() + "/" + $("#ddlLotes").val() + "/" + generarFechaOrdenado($("#ddlFecha").val());
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarComponentes, BuscarComponentesError, 10000);
}

function RespuestaBuscarComponentes(data) {
    console.log(data);
    LlenarTablasComponentes(data);

}
function BuscarComponentesError(data) {
    console.log(data);
}




function LlenarTablasComponentes(data) {
    var html = "";
    if (data.length == 0) {
        html += '<tr>' +
            '<td colspan="4" class="text-center">No existe registros</td>' +
            '</tr>';
    } else if (data.filter(e => e.lote == null).length > 0) {
        html += '<tr>' +
            '<td colspan="4" class="text-center">No existe registros</td>' +
            '</tr>';
    } else {
        for (i = 0; i < data.length; i++) {
            var porcentaje = data[i].porcentaje != null ? data[i].porcentaje : "";
            html += '<tr>';
            html += "<td>" + data[i].lote + "</td>";
            html += "<td>" + data[i].suministrador + "</td>";
            html += "<td class='text-rigth'>" + porcentaje + "</td>";
            html += "<td>" + data[i].fechaCadena + "</td>";
            html += "</tr>";
        }
    }
    $("#tblComponentes tbody").html(html);

}


function BuscarNuevo() {
    var url = $("#__URL_LISTAR_COMPONENTE").val() + "/" + $("#ddlLotes").val() + "/" + generarFechaOrdenado($("#__HD_FECHA").val());
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarNuevo, BuscarNuevoError, 10000);
}

function RespuestaBuscarNuevo(data) {    
    pintarNuevoRegistroComponente(data);

}
function BuscarNuevoError(data) {
    console.log(data);
}

function pintarNuevoRegistroComponente(data) {
    var html = "";
    console.log(data);
    console.log($("#__HD_FECHA").val());
    for (var i = 0; i < data.length; i++) {
        var porcentajeValidar = data[i].porcentaje != null ? data[i].porcentaje : "";
        var percentaje = "";
        console.log(data[i].fechaCadena);
        if (generarFechaOrdenado($("#__HD_FECHA").val()) == generarFechaOrdenado(data[i].fechaCadena)) {
            percentaje = $("#__HD_FECHA").val() == data[i].porcentaje;
        }
        
        html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].idSuministrador + '" >' +
            '<td> <input type="text" class="form-control form-report" value="' + $("#ddlLotes option:selected").text() + '" disabled ></td>' +
            '<td> <input type="text" class="form-control form-report" value="' + data[i].suministrador + '" disabled></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="compPorcentaje_' + data[i].idSuministrador + '" value="' + percentaje + '" ></td>' +
            '<td> <input type="text" class="form-control form-report" value="' + $("#ddlFecha").val() + '" disabled></td>' +
            '</tr>';
    }
    $("#tblNuevoComponentes tbody").html(html);
}



function IrDetalleReporteError(data) {
    console.log(data);
    MensajeAlerta("No se pudo obtener el registro, intente nuevamente y comunicarse con soporte", "error");
}


function Guardar() {
    if ($("#ddlLotes").val().length == 0) {
        MensajeAlerta("Lote es requerido", "error");
        return;
    } else if ($("#ddlFecha").val().length == 0) {
        MensajeAlerta("Fecha es requerido", "error");
        return;
    }
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);

    var url = $("#__URL_GUARDAR").val() + "/" + $("#ddlLotes").val() + "/" + generarFechaOrdenado($("#ddlFecha").val());
    var component = [];
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        component.push({
            idSuministrador: datoId,
            idLote: $("#ddlLotes").val(),
            porcentaje: $("#compPorcentaje_" + datoId).val().length > 0 ? $("#compPorcentaje_" + datoId).val() : null,
        });
    });
    realizarPost(url, component, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
    //BuscarValores();
}

function GuardarError(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("No se pudo completar, intente nuevamente y comunicarse con soporte", "error");
}
