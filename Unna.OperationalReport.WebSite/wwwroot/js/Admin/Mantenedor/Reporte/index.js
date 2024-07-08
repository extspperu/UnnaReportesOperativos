$(document).ready(function () {
    controles();
});


function controles() {
    $('.campo-fecha').datepicker({ format: "dd/mm/yyyy" });
    $('.campo-fecha').datepicker().on('changeDate', function (ev) { $('.campo-fecha').datepicker('hide'); });
    $('#selectReporte').change(function () {
        ObtenerDatosReporte();
    });
    $('#btnGuardarReporte').click(function () {
        GuardarReporte();
    });


    BuscarReportes();
}



function BuscarReportes() {
    var url = $("#__URL_LISTAR_REPORTES").val();
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




var tablaReportes = null;
function LlenarTablasReportes(data) {
    if (tablaReportes) {
        tablaReportes.destroy();
        tablaReportes = null;
    }
    var table = $('#tblReportes').DataTable();
    table.destroy();

    var html = "";
    for (i = 0; i < data.length; i++) {

        html += '<tr>';
        html += "<td>" + '<button onclick="IrDetalleReporte(\'' + data[i].id + '\')" class="btn btn-sm btn-clean btn-icon mr-1" title="Editar">\
									<i class="la la-edit"></i>\
								</button></td>';
        html += "<td>" + data[i].grupo + "</td>";
        html += "<td>" + data[i].nombreReporte + "</td>";

        var nombre = "";
        if (data[i].nombre != null) {
            nombre = data[i].nombre;
        }
        html += "<td>" + nombre + "</td>";

        var version = "";
        if (data[i].version != null) {
            version = data[i].version;
        }
        html += "<td>" + version + "</td>";

        var preparado = "";
        if (data[i].preparadoPor != null) {
            preparado = data[i].preparadoPor;
        }
        var aprobadoPor = "";
        if (data[i].aprobadoPor != null) {
            aprobadoPor = data[i].aprobadoPor;
        }
        html += "<td>" + preparado + "/" + aprobadoPor + "</td>";

        var fechaCadena = "";
        if (data[i].fechaCadena != null) {
            fechaCadena = data[i].fechaCadena;
        }
        html += "<td>" + fechaCadena + "</td>";
        html += "</tr>";


    }
    $("#tblReportes tbody").html(html);
    tablaReportes = $('#tblReportes').DataTable({
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
    var url = $("#__URL_OBTENER_REPORTES").val() + id;
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaIrDetalleReporte, IrDetalleReporteError, 10000);
}

function RespuestaIrDetalleReporte(data) {
    $("#modalReporteAdministrar").modal("show");
    console.log(data);
    $("#tbAprobado").val(data.aprobadoPor);
    $("#tbPreparadoPor").val(data.preparadoPor);
    $("#tbCompania").val(data.nombre);
    $("#tbNombreReporte").val(data.nombreReporte);
    $("#tbIdReporte").val(data.id);
    $("#tbGrupo").val(data.grupo);
    $("#tbDetalle").val(data.detalle);
    $("#tbVersion").val(data.version);
    if (data.fecha != null) {
        var fecha = new Date(data.fecha)
        $("#tbFecha").val(fecha.getDate() + "/" + (fecha.getMonth() + 1) + "/" + fecha.getFullYear());
    }

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
