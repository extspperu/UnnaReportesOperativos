$(document).ready(function () {
    controles();
});


function controles() {
  
    $('#btnGuardarReporte').click(function () {
        GuardarReporte();
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
    //$("#tbAprobado").val(data.llave);

}

function IrDetalleReporteError(data) {
    console.log(data);
    MensajeAlerta("No se pudo obtener el registro, intente nuevamente y comunicarse con soporte", "error");
}


function GuardarReporte() {
    $("#btnGuardarReporte").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardarReporte").prop("disabled", true);
    var url = $("#__URL_GUARDAR_REPORTES").val();
    var dato = {
        aprobadoPor: $("#tbAprobado").val(),
        preparadoPor: $("#tbPreparadoPor").val(),
        nombre: $("#tbCompania").val(),
        nombreReporte: $("#tbNombreReporte").val(),
        id: $("#tbIdReporte").val(),
        grupo: $("#tbGrupo").val(),
        detalle: $("#tbDetalle").val(),
        version: $("#tbVersion").val(),
        fecha: $("#tbFecha").val().length > 0 ? generarFechaOrdenado($("#tbFecha").val()) : null,
    };
    realizarPost(url, dato, 'json', RespuestaGuardarReporte, GuardarReporteError, 10000);
}

function RespuestaGuardarReporte(data) {
    $("#btnGuardarReporte").html('Guardar');
    $("#btnGuardarReporte").prop("disabled", false);
    $("#modalReporteAdministrar").modal("hide");
    MensajeAlerta("Se guardó correctamente", "success");
    BuscarReportes();
}

function GuardarReporteError(data) {
    $("#btnGuardarReporte").html('Guardar');
    $("#btnGuardarReporte").prop("disabled", false);
    console.log(data);
    MensajeAlerta("No se pudo completar, intente nuevamente y comunicarse con soporte", "error");
}
function Limpiar() {
    $("#tbAprobado").val("");
    $("#tbPreparadoPor").val("");
    $("#tbCompania").val("");
    $("#tbNombreReporte").val("");
    $("#tbIdReporte").val("");
    $("#tbGrupo").val("");
    $("#tbDetalle").val("");
    $("#tbVersion").val("");
    $("#tbFecha").val("");
}

function generarFechaOrdenado(fecha) {
    const [day2, month2, year2] = fecha.split('/');
    const hasta = [year2, month2, day2].join('-');
    return hasta;
}
