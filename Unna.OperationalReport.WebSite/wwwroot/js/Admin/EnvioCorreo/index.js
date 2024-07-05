$(document).ready(function () {
    controles();
});


function controles() {
    $('#selectReporte').change(function () {
        ObtenerDatosReporte();
    });
    $('#btnDescargarPdf').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Pdf/" + $("#selectReporte").val();
    });
    $('#btnDescargarExcel').click(function () {
        document.location = $("#__URL_DESCARGAR_DOCUMENTOS").val() + "Excel/" + $("#selectReporte").val();
    });
    $('#btnOpenOutlook').click(function (event) {
        var email = 'mvillanueva@gmail.com';
        var subject = 'Test';
        var emailBody = 'Hi Sample,';
        //document.location = "mailto:" + email + "?subject=" + subject + "&body=" + emailBody + "&attach=" + attach;
        document.location = 'mailto:' + email + '?subject=' + subject + '&body=' + emailBody;
    });

    //ListarReportes();
}


function ListarReportes() {
    var url = $('#__URL_LISTAR_REPORTES').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaListarReportes, ErrorListarReportes, 10000);
}

function RespuestaListarReportes(data) {
    //Guardar(data);
    console.log(data);


    for (var i = 0; i < data.length; i++) {
        $("#selectReporte").append('<option value="' + data[i].id + '">' + data[i].nombreReporte + '</option>');
    }
}

function ErrorListarReportes(data) {
    //Guardar(data);
    console.log(data);
}


function ObtenerDatosReporte() {
    $("#loadingContenidoCorreo").show();
    $("#contenidoCorreo").hide();
    limpiarContenido();
    var url = $('#__URL_DATOS_REPORTE').val() + $('#selectReporte').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtenerDatosReporte, ErrorObtenerDatosReporte, 10000);
}

function RespuestaObtenerDatosReporte(data) {
    $("#loadingContenidoCorreo").hide();
    $("#contenidoCorreo").show();
    console.log(data);
    if (data.fueEnviado) {
        $("#contenidoErrorBuscarReporte").show()
    } else {
        $("#contenidoSuccesBuscarReporte").show()
    }

    $("#mensajeErrorBuscarReporte").html(data.mensajeAlert)
    $("#mensajeSuccesBuscarReporte").html(data.mensajeAlert)
    if (data.tieneArchivoExcel) {
        $("#btnDescargarExcel").show()
    }
    if (data.tieneArchivoPdf) {
        $("#btnDescargarPdf").show()
    }

}

function ErrorObtenerDatosReporte(data) {
    $("#loadingContenidoCorreo").hide();
    console.log(data);
}


function limpiarContenido() {
    $("#btnDescargarExcel").hide();
    $("#btnDescargarPdf").hide();
    $("#tbDestinatario").val("");
    $("#tbCc").val("");
    $("#tbAsunto").val("");
    $("#tbCuerpoCorreo").val("");
}
