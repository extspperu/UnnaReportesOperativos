$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnOpenAgregarUsuario').click(function () {
        NuevoUsuario();
    });
    $('#btnGuardarUsuario').click(function () {
        GuardarReporte();
    });
    $('#checkUsuarioExterno').change(function () {
        if ($('#checkUsuarioExterno').prop('checked')) {
            $(".checkUsuarioExterno").show();
            $("#tbPassword").val("");
            $("#tbConfirmarPassword").val("");
        } else {
            $(".checkUsuarioExterno").hide();
        }
    });
    $('#tbGrupo').change(function () {
        document.getElementById('tbLote').selectedIndex = 0;
        if ($('#tbGrupo').val() == $('#_HD_FISCALIZADOR_REGULAR').val() || $('#tbGrupo').val() == $('#_HD_FISCALIZADOR_ENEL').val()) {            
            $("#tbLote").show();
        } else {
            $("#tbLote").hide();
        }
    });
    $('#btnCerrarUsuario, #btnCloseMd').click(function () {
        $("#modalUsuarioAdministrar").modal("hide");
        ListarGrupos();
        ListarLotes();
    });

    BuscarUsuarios();
    ListarGrupos();
    ListarLotes();
}

function ListarGrupos() {
    var url = $("#__URL_LISTAR_GRUPOS").val();
    var dato = {

    };
    realizarGet(url, dato, 'json', RespuestaListarGrupos, ListarGruposError, 10000);
}

function RespuestaListarGrupos(data) {

    var html = '<option value="">--Seleccione--</option>';
    for (var i = 0; i < data.length; i++) {
        html += '<option value="' + data[i].id + '">' + data[i].nombre + '</option>';
    }
    $("#tbGrupo").html(html);
}

function ListarGruposError(data) {
    console.log(data);
}

function ListarLotes() {
    var url = $("#__URL_LISTAR_LOTES").val();
    var dato = {

    };
    realizarGet(url, dato, 'json', RespuestaListarLotes, ListarLotesError, 10000);
}

function RespuestaListarLotes(data) {

    var html = '<option value="">--Seleccione--</option>';
    for (var i = 0; i < data.length; i++) {
        html += '<option value="' + data[i].id + '">' + data[i].nombre + '</option>';
    }
    $("#tbLote").html(html);
}

function ListarLotesError(data) {
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
        html += "<td>" + grupo + "</td>";
        html += "<td>" + (data[i].estaHabilitado ? 'Si' : 'No') + "</td>";
        html += "<td>" + (data[i].esAdministrador ? 'Si' : 'No') + "</td>";
        html += "<td>" + data[i].creado + "</td>";
        var ultimoLogin = "";
        if (data[i].ultimoLogin != null) {
            ultimoLogin = data[i].ultimoLogin;
        }
        html += "<td>" + ultimoLogin + "</td>";
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
    $("#tbIdUsuario").val(data.idUsuarioCifrado);
    $("#tbDocumento").val(data.documento);
    $("#tbNombres").val(data.nombres);
    $("#tbPaterno").val(data.paterno);
    $("#tbMaterno").val(data.materno);
    $("#tbTelefono").val(data.telefono);
    $("#tbCorreo").val(data.username);
    $('#checkEstaHabilitado').prop('checked', data.estaHabilitado);
    $('#checkEsAdministrador').prop('checked', data.esAdministrador);
    $('#checkUsuarioExterno').prop('checked', data.esUsuarioExterno);
    $("#tbGrupo option[value=" + data.idGrupoCifrado + "]").attr("selected", true);

    if (data.idGrupoCifrado == $('#_HD_FISCALIZADOR_REGULAR').val() || data.idGrupoCifrado == $('#_HD_FISCALIZADOR_ENEL').val()) {
        $("#tbLote").show();
        $("#tbLote option[value=" + data.idLote + "]").attr("selected", true);
    } else {
        $("#tbLote").hide();
    }

    if (data.esUsuarioExterno) {
        $(".checkUsuarioExterno").show();
        $("#tbPassword").val("******");
        $("#tbConfirmarPassword").val("******");
    } else {
        $(".checkUsuarioExterno").hide();
    }
}

function DetalleUsuarioError(data) {
    console.log(data);
    MensajeAlerta("No se pudo obtener el registro, intente nuevamente y comunicarse con soporte", "error");
}
function validarCamposRequeridos() {
    var flat = true;
    if ($("#tbDocumento").val().length === 0) {
        $("#tbDocumento").focus();
        MensajeAlerta("Documento es requerido", "info");
        flat = false;
    } else if ($("#tbNombres").val().length === 0) {
        $("#tbNombres").focus();
        MensajeAlerta("Nombres es requerido", "info");
        flat = false;
    } else if ($("#tbPaterno").val().length === 0) {
        $("#tbPaterno").focus();
        MensajeAlerta("Paterno es requerido", "info");
        flat = false;
    } else if ($("#tbMaterno").val().length === 0) {
        $("#tbMaterno").focus();
        MensajeAlerta("Materno es requerido", "info");
        flat = false;
    } else if ($("#tbCorreo").val().length === 0) {
        $("#tbCorreo").focus();
        MensajeAlerta("Correo es requerido", "info");
        flat = false;
    } else if ($("#tbPassword").val().length === 0 && $('#checkUsuarioExterno').prop('checked')) {
        $("#tbPassword").focus();
        MensajeAlerta("Contraseña es requerido", "info");
        flat = false;
    } else if ($("#tbConfirmarPassword").val().length === 0 && $('#checkUsuarioExterno').prop('checked')) {
        $("#tbConfirmarPassword").focus();
        MensajeAlerta("Confirme contraseña", "info");
        flat = false;
    }
    return flat;
}

function GuardarReporte() {
    if (validarCamposRequeridos()) {
        $("#btnGuardarUsuario").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
        $("#btnGuardarUsuario").prop("disabled", true);
        var url = $("#__URL_GUARDAR_USUARIO").val();
        var dato = {
            IdUsuario: $("#tbIdUsuario").val(),
            Username: $("#tbCorreo").val(),
            IdGrupo: $("#tbGrupo").val(),
            EstaHabilitado: $('#checkEstaHabilitado').prop('checked'),
            EsAdministrador: $('#checkEsAdministrador').prop('checked'),
            Documento: $("#tbDocumento").val(),
            Paterno: $("#tbPaterno").val(),
            Materno: $("#tbMaterno").val(),
            Nombres: $("#tbNombres").val(),
            Telefono: $("#tbTelefono").val(),
            Correo: $("#tbCorreo").val(),
            Password: $("#tbPassword").val(),
            PasswordConfirmar: $("#tbConfirmarPassword").val(),
            EsUsuarioExterno: $('#checkUsuarioExterno').prop('checked'),
            idLote: $("#tbLote").val(),
        };
        realizarPost(url, dato, 'json', RespuestaGuardarReporte, GuardarReporteError, 10000);
    }
}

function RespuestaGuardarReporte(data) {
    $("#btnGuardarUsuario").html('Guardar');
    $("#btnGuardarUsuario").prop("disabled", false);
    $("#modalUsuarioAdministrar").modal("hide");
    MensajeAlerta("Se guardó correctamente", "success");
    BuscarUsuarios();
    ListarGrupos();
    ListarLotes();
}

function GuardarReporteError(data) {
    $("#btnGuardarUsuario").html('Guardar');
    $("#btnGuardarUsuario").prop("disabled", false);
    console.log(data);
    MensajeAlerta(data.responseJSON.mensajes[0], "error");
}
function Limpiar() {
    document.getElementById('tbGrupo').selectedIndex = 0;
    document.getElementById('tbLote').selectedIndex = 0;

    $('#tbGrupo option').prop('selected', false);
    $('#tbLote option').prop('selected', false);
    $("#tbIdUsuario").val("");
    $("#tbDocumento").val("");
    $("#tbNombres").val("");
    $("#tbPaterno").val("");
    $("#tbMaterno").val("");
    $("#tbTelefono").val("");
    $("#tbCorreo").val("");
    $('#checkEstaHabilitado').prop('checked', false);
    $('#checkEsAdministrador').prop('checked', false);
    $('#checkUsuarioExterno').prop('checked', false);
    
    $(".checkUsuarioExterno").hide();
}
