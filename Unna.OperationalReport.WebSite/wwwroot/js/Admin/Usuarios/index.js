$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnOpenAgregarUsuario').click(function () {
        NuevoUsuario();
    });
    $('#btnGuardarReporte').click(function () {
        GuardarReporte();
    });


    BuscarUsuarios();
    ListarGrupos();
}

function ListarGrupos() {
    var url = $("#__URL_LISTAR_GRUPOS").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaListarGrupos, ListarGruposError, 10000);
}

function RespuestaListarGrupos(data) {
    console.log(data);
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<option value="' + data[i].id + '">' + data[i].nombre + '</option>';
    }
    $("#tbGrupo").append(html);
}

function ListarGruposError(data) {
    console.log(data);
}

function NuevoUsuario() {
    Limpiar();
    $("#modalUsuarioAdministrar").modal("show");
}

function BuscarUsuarios() {
    var url = $("#__URL_LISTAR_USUARIOS").val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaBuscarUsuarios, BuscarUsuariosError, 10000);
}

function RespuestaBuscarUsuarios(data) {
    console.log(data);
    LlenarTablasUsuarios(data);

}
function BuscarUsuariosError(data) {
    console.log(data);
}


var tablaUsuarios = null;
function LlenarTablasUsuarios(data) {
    if (tablaUsuarios) {
        tablaUsuarios.destroy();
        tablaUsuarios = null;
    }
    var table = $('#tblUsuarios').DataTable();
    table.destroy();

    var html = "";
    for (i = 0; i < data.length; i++) {

        html += '<tr>';
        html += "<td>" + '<button onclick="IrDetalleUsuario(\'' + data[i].idUsuario + '\')" class="btn btn-sm btn-clean btn-icon mr-1" title="Editar">\
									<i class="la la-edit"></i>\
								</button></td>';
        html += "<td>" + data[i].username + "</td>";

        var documento = "";
        if (data[i].documento != null) {
            documento = data[i].documento;
        }
        html += "<td>" + documento + "</td>";

        var nombres = "";
        if (data[i].nombres != null) {
            nombres = data[i].nombres;
        }
        html += "<td>" + nombres + "</td>";

        var grupo = "";
        if (data[i].grupo != null) {
            grupo = data[i].grupo;
        }
        html += "<td>" + data[i].grupo + "</td>";
        html += "<td>" + (data[i].estaHabilitado ? 'Si' : 'No') + "</td>";
        html += "<td>" + (data[i].esAdministrador ? 'Si' : 'No') + "</td>";
        html += "</tr>";
    }
    $("#tblUsuarios tbody").html(html);
    tablaUsuarios = $('#tblUsuarios').DataTable({
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



function IrDetalleUsuario(id) {
    Limpiar();
    var url = $("#__URL_OBTENER_USUARIO").val() + id;
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaDetalleUsuario, DetalleUsuarioError, 10000);
}

function RespuestaDetalleUsuario(data) {
    $("#modalUsuarioAdministrar").modal("show");
    console.log(data);
    $("#tbIdUsuario").val(data.idUsuario);
    $("#tbDocumento").val(data.documento);
    $("#tbNombres").val(data.nombres);
    $("#tbPaterno").val(data.paterno);
    $("#tbMaterno").val(data.materno);
    $("#tbTelefono").val(data.telefono);
    $("#tbCorreo").val(data.username); 
    $('#checkEstaHabilitado').prop('checked', data.estaHabilitado);
    $('#checkEsAdministrador').prop('checked', data.esAdministrador);
    $("#tbGrupo option[value=" + data.idGrupoCifrado + "]").attr("selected", true);
}

function DetalleUsuarioError(data) {
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
    $("#tbIdUsuario").val("");
    $("#tbDocumento").val("");
    $("#tbNombres").val("");
    $("#tbPaterno").val("");
    $("#tbMaterno").val("");
    $("#tbTelefono").val("");
    $("#tbCorreo").val("");
    $('#checkEstaHabilitado').prop('checked', false);
    $('#checkEsAdministrador').prop('checked', false);    
    document.getElementById('tbGrupo').selectedIndex = 0;
}
