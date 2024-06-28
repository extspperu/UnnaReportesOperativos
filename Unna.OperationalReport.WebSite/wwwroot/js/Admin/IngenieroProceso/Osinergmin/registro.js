$(document).ready(function () {
    controles();
});
function controles() {
    $('#btnCargarReporte').change(function () {
        CargarArchivo();
    });
    $('#btnGuardar').click(function () {
        Guardar();
    });

    Obtener();
}

function Guardar() {

    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
    var url = $('#__URL_GUARDAR_REGISTRO').val();
    var dara = {
        username: $("#tbUsuario").val(),
        password: $("#tbContrasenia").val(),
        nacionalizadaM3: $("#tbM3").val(),
        nacionalizadaTn: $("#tbTn").val(),
        nacionalizadaBbl: $("#tbBbl").val(),
        archivo: {
            id: $("#__HD_ID_ARCHIVO").val()
        },
    };
    realizarPost(url, dara, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('GUARDAR');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");

}

function GuardarError(data) {
    console.log(data);
    $("#btnGuardar").html('GUARDAR');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = data.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}


function CargarArchivo() {
    $("#btnCargarReporteLabel").prop("disabled", true);
    $("#btnCargarReporteLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_DOCUMENTO').val();
    var dato = new FormData($("#FormCargarReporte")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnCargarReporteLabel").html("Cargar");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
            console.log(data);
            $("#__HD_ID_ARCHIVO").val(data.id);
            pintarDescargarArchivo(data);
        },
        error: function (jqXHR, status, error) {
            $("#btnCargarReporteLabel").html("Cargar");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
        },
    });
}

function pintarDescargarArchivo(data) {
    var html = "";
    if (data != null) {
        html = '<p class="d-flex justify-content-between" style="border: 0px solid #fff;font-size: 18px;font-weight: 600;margin-bottom: 0px;"><a href="' + data.url + '" >' + data.nombre + '</a> <a class="cerrar" href="javascript:void(0)" onclick="quitarReporteCargado()">x</a></p>';
    }
    $("#cargarReporteResultado").html(html);
}

function quitarReporteCargado() {
    $("#__HD_ID_ARCHIVO").val("");
    $("#cargarReporteResultado").html("");
}

function Obtener() {
    var url = $('#__URL_OBTENER_REGISTRO').val();
    var dara = {
    };
    realizarGet(url, dara, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#tbUsuario").val(data.username);
    $("#tbContrasenia").val(data.password);
    $("#tbM3").val(data.nacionalizadaM3);
    $("#tbTn").val(data.nacionalizadaTn);
    $("#tbBbl").val(data.nacionalizadaBbl);
    if (data.archivo != null) {
        $("#__HD_ID_ARCHIVO").val(data.archivo.id);
        pintarDescargarArchivo(data.archivo);
    }


}

function ObtenerError(data) {
    console.log(data);

}