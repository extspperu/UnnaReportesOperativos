﻿$(document).ready(function () {
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
    console.log(data);
    $("#tbIdUsuario").val(data.idUsuarioCifrado);
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
            EstaHabilitado: $('#checkUsuarioExterno').prop('checked'),
            EsAdministrador: $('#checkUsuarioExterno').prop('checked'),
            Documento: $("#tbDocumento").val(),
            Paterno: $("#tbPaterno").val(),
            Materno: $("#tbMaterno").val(),
            Nombres: $("#tbNombres").val(),
            Telefono: $("#tbTelefono").val(),
            Correo: $("#tbCorreo").val(),
            Password: $("#tbPassword").val(),
            PasswordConfirmar: $("#tbConfirmarPassword").val(),
            EsUsuarioExterno: $('#checkUsuarioExterno').prop('checked'),
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
}

function GuardarReporteError(data) {
    $("#btnGuardarUsuario").html('Guardar');
    $("#btnGuardarUsuario").prop("disabled", false);
    console.log(data);
    MensajeAlerta(data.responseJSON.mensajes[0], "error");
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
    $('#checkUsuarioExterno').prop('checked', false);    
    document.getElementById('tbGrupo').selectedIndex = 0;
    $(".checkUsuarioExterno").hide();
}
