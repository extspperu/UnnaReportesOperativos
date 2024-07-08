$(document).ready(function () {
    controles();
});


function controles() {

    $('#btnGuardarReporte').click(function () {
        Guardar();
    });
    $('#btnNuevoRegistro').click(function () {
        Limpiar();
        $("#tbLlave").prop("disabled", false);
        $("#modalValoresGenerales").modal("show");
    });


    BuscarValores();
}



function BuscarValores() {
    var url = $("#__URL_LISTAR_VALORES").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarReportes, BuscarReportesError, 10000);
}

function RespuestaBuscarReportes(data) {
    console.log(data);
    LlenarTablasReportes(data);

}
function BuscarReportesError(data) {
    console.log(data);
}




var tablaValores = null;
function LlenarTablasReportes(data) {
    if (tablaValores) {
        tablaValores.destroy();
        tablaValores = null;
    }
    var table = $('#tblValores').DataTable();
    table.destroy();

    var html = "";
    for (i = 0; i < data.length; i++) {

        html += '<tr>';
        html += "<td>" + '<button onclick="IrDetalleReporte(\'' + data[i].id + '\')" class="btn btn-sm btn-clean btn-icon mr-1" title="Editar">\
									<i class="la la-edit"></i>\
								</button></td>';

        html += "<td>" + data[i].comentario + "</td>";
        html += "<td>" + data[i].valor + "</td>";

        var habilitado = "No";
        if (data[i].estaHabilitado) {
            habilitado = "Si";
        }
        html += "<td>" + habilitado + "</td>";
        html += "<td>" + data[i].creado + "</td>";
        html += "<td>" + data[i].actualizado + "</td>";
        html += "</tr>";


    }
    $("#tblValores tbody").html(html);
    tablaValores = $('#tblValores').DataTable({
        "searching": true,
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



function IrDetalleReporte(id) {
    Limpiar();
    var url = $("#__URL_OBTENER_VALORES").val() + id;
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaIrDetalleReporte, IrDetalleReporteError, 10000);
}

function RespuestaIrDetalleReporte(data) {
    $("#modalValoresGenerales").modal("show");
    console.log(data);
    $("#tbIdValor").val(data.id);
    $("#tbNombre").val(data.comentario);
    $("#tbValor").val(data.valor);
    $("#tbLlave").val(data.llave);
    $("#tbCreado").val(data.creado);
    $("#tbActualizado").val(data.actualizado);
    $('#checkEstaHabilitado').prop('checked', data.estaHabilitado);
    $("#tbLlave").prop("disabled", true);

}

function IrDetalleReporteError(data) {
    console.log(data);
    MensajeAlerta("No se pudo obtener el registro, intente nuevamente y comunicarse con soporte", "error");
}


function Guardar() {
    if ($("#tbNombre").val().length == 0) {
        MensajeAlerta("Nombre es requerido", "error");
        return;
    } else if ($("#tbValor").val().length == 0) {
        MensajeAlerta("Valor es requerido", "error");
        return;
    } else if ($("#tbLlave").val().length == 0 && $("#tbIdValor").val().length == 0) {
        MensajeAlerta("Llave es requerido", "error");
        return;
    }
    $("#btnGuardarReporte").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardarReporte").prop("disabled", true);
    var url = $("#__URL_GUARDAR_VALORES").val();
    var dato = {
        id: $("#tbIdValor").val(),
        comentario: $("#tbNombre").val(),
        valor: $("#tbValor").val(),
        llave: $("#tbLlave").val(),
        estaHabilitado: $('#checkEstaHabilitado').prop('checked')

    };
    realizarPost(url, dato, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardarReporte").html('Guardar');
    $("#btnGuardarReporte").prop("disabled", false);
    $("#modalValoresGenerales").modal("hide");
    MensajeAlerta("Se guardó correctamente", "success");
    BuscarValores();
}

function GuardarError(data) {
    $("#btnGuardarReporte").html('Guardar');
    $("#btnGuardarReporte").prop("disabled", false);
    MensajeAlerta("No se pudo completar, intente nuevamente y comunicarse con soporte", "error");
}
function Limpiar() {
    $("#tbIdValor").val("");
    $("#tbNombre").val("");
    $("#tbValor").val("");
    $("#tbLlave").val("");
    $('#checkEstaHabilitado').prop('checked', false);
}
