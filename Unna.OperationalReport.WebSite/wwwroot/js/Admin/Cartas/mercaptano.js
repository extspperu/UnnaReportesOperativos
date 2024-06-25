var parametros;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnGuardar').click(function () {
        Guardar();
    });
    $('#btnGuardarArchivos').click(function () {
        GuardarArchivos();
    });

    $('#btnAgregarPeriodoPrecion').click(function () {
        agregarPeriodoPrecio();
    });
    $('#btnDescargarPdf').click(function () {
        DescargarDocumentoPdf();
    });
    $('#tbNivelInicial, #tbNivelFinal,#tbVolumenReposicionGal,#tbDespachoGalones,#tbDespachoBarril, #tbGravedadEspecificaEtil, #tbDensidadDelAgua,#tbConsumoMensual,#tbCantidadDosificadaGal, #tbCantidadDosificadaM3,#tbVolumenGlpM3, #tbGravedadEspecificaEtil, #tbDensidadDelAgua').keyup(function () {
        calculosReporte();
    });


    //("#tbNivelInicial").val(data.nivelInicial);
    //$("#tbNivelFinal").val(data.nivelFinal);
    //$("#tbDespachoBarril").val(data.despachoBarril);
    //$("#tbVolumenGlpBarriles").val(data.volumenGlpBarriles);
    //$("#tbVolumenGlpM3").val(data.volumenGlpM3);
    //$("#tbCantidadDosificadaM3").val(data.cantidadDosificadaM3);


    Obtener();
}


function Obtener() {
    $("#contenidoErrorMensaje").hide();
    $("#contenidoCarta").hide();
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {

    };
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    //Guardar(data);
    console.log(data);
    $("#contenidoCarta").show();
    parametros = data;
    $("#firmaCarta").html('<img src="' + data.urlFirma + '" style="max-width:160px;" />');

    cargarSolicitud(data.solicitud);
    cargarMercaptano(data.mercaptano);
}

function ErrorObtener(data) {
    //Guardar(data);
    console.log(data);
    $("#contenidoErrorMensaje").show();
}


function cargarSolicitud(data) {
    //Guardar(data);

    $("#tbSumilla").val(data.sumilla);
    $("#tbNumero").val(data.numero);
    $("#tbAnio").val(data.anio);
    $("#tbFecha").val(data.fecha);
    $("#tbPeriodo").val(data.periodo);
    $("#tbDestinatario").val(data.destinatario);
    $("#tbAsunto").val(data.asunto);
    $("#tbCuerpo").val(data.cuerpo);
    $("#tbPie").val(data.pie);

}

function cargarMercaptano(data) {

    $("#tbFechaMercaptano").html(data.fecha);
    $("#tbSupervisorResponsable").html(data.reponsable);

    $("#tbNivelInicial").val(data.nivelInicial);
    $("#tbFechaInicial").val(data.fechaInicial);
    $("#tbLitrosInicial").val(data.litrosInicial);
    $("#tbNivelFinal").val(data.nivelFinal);
    $("#tbFechaFinal").val(data.fechaFinal);
    $("#tbLitrosFinal").val(data.litrosFinal);
    $("#tbVolumenReposicionGal").val(data.volumenReposicionGal);
    $("#tbVolumenReposicionLitros").val(data.volumenReposicionLitros);
    $("#tbConsumoLitros").val(data.consumoLitros);
    $("#tbDespachoBarril").val(data.despachoBarril);
    $("#tbDespachoGalones").val(data.despachoGalones);
    $("#tbVolumenGlpBarriles").val(data.volumenGlpBarriles);
    $("#tbVolumenGlpM3").val(data.volumenGlpM3);
    $("#tbCantidadDosificadaM3").val(data.cantidadDosificadaM3);
    $("#tbCantidadDosificadaGal").val(data.cantidadDosificadaGal);
    $("#tbConsumoMensual").val(data.consumoMensual);
    $("#tbNotaMercaptano").val(data.nota);
    $("#tbGravedadEspecificaEtil").val(data.gravedadEspecificaEtil);
    $("#tbDensidadDelAgua").val(data.densidadDelAgua);

}






function DescargarDocumentoPdf() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GENERAR_PDF').val();
    parametros.solicitud.sumilla = $("#tbSumilla").val();
    parametros.solicitud.numero = $("#tbNumero").val();
    parametros.solicitud.anio = $("#tbAnio").val();
    parametros.solicitud.fecha = $("#tbFecha").val();
    parametros.solicitud.periodo = $("#tbPeriodo").val();
    parametros.solicitud.destinatario = $("#tbDestinatario").val();
    parametros.solicitud.asunto = $("#tbAsunto").val();
    parametros.solicitud.cuerpo = $("#tbCuerpo").val();
    parametros.solicitud.pie = $("#tbPie").val();

    parametros.mercaptano.nivelInicial = $("#tbNivelInicial").val();
    parametros.mercaptano.litrosInicial = $("#tbLitrosInicial").val();
    parametros.mercaptano.nivelFinal = $("#tbNivelFinal").val();
    parametros.mercaptano.litrosFinal = $("#tbLitrosFinal").val();

    parametros.mercaptano.volumenReposicionGal = $("#tbVolumenReposicionGal").val();
    parametros.mercaptano.volumenReposicionLitros = $("#tbVolumenReposicionLitros").val();
    parametros.mercaptano.consumoLitros = $("#tbConsumoLitros").val();
    parametros.mercaptano.despachoBarril = $("#tbDespachoBarril").val();
    parametros.mercaptano.despachoGalones = $("#tbDespachoGalones").val();
    parametros.mercaptano.volumenGlpBarriles = $("#tbVolumenGlpBarriles").val();
    parametros.mercaptano.volumenGlpM3 = $("#tbVolumenGlpM3").val();
    parametros.mercaptano.cantidadDosificadaM3 = $("#tbCantidadDosificadaM3").val();
    parametros.mercaptano.cantidadDosificadaGal = $("#tbCantidadDosificadaGal").val();
    parametros.mercaptano.consumoMensual = $("#tbConsumoMensual").val();
    parametros.mercaptano.nota = $("#tbNotaMercaptano").val();
    parametros.mercaptano.gravedadEspecificaEtil = $("#tbGravedadEspecificaEtil").val();
    parametros.mercaptano.densidadDelAgua = $("#tbDensidadDelAgua").val();

    realizarPost(url, parametros, 'json', RespuestaDescargarDocumentoPdf, RespuestaDescargarDocumentoPdfError, 10000);
}

function RespuestaDescargarDocumentoPdf(data) {
    window.location = $("#__URL_DESCARGAR_PDF").val() + data.key;
    //$("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    //$("#btnGuardar").prop("disabled", false);
}

function RespuestaDescargarDocumentoPdfError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    //$("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    //$("#btnGuardar").prop("disabled", false);

}

function calculosReporte() {
    var nivelInicial = $("#tbNivelInicial").val().length > 0 ? parseFloat($("#tbNivelInicial").val()) : 0;
    var litrosInicial = 3.1416 * (30 * 30) * (nivelInicial) / 1000;
    $("#tbLitrosInicial").val(litrosInicial.toFixed(2));

    var nivelFinal = $("#tbNivelFinal").val().length > 0 ? parseFloat($("#tbNivelFinal").val()) : 0;
    var litrosFinal = 3.1416 * (30 * 30) * (nivelFinal) / 1000;
    $("#tbLitrosFinal").val(litrosFinal.toFixed(2));

    var volumenReposicion = $("#tbVolumenReposicionGal").val().length > 0 ? parseFloat($("#tbVolumenReposicionGal").val()) : 0;
    var volumenReposicionLitros = 3.785 * volumenReposicion;
    $("#tbVolumenReposicionLitros").val(volumenReposicionLitros.toFixed(2));

    var consumoLitros = litrosInicial - litrosFinal + volumenReposicionLitros;
    $("#tbConsumoLitros").val(consumoLitros.toFixed(1));

    var despachoGalones = $("#tbDespachoGalones").val().length > 0 ? parseFloat($("#tbDespachoGalones").val()) : 0;
    var despachoBarril = despachoGalones / 42;
    $("#tbDespachoBarril").val(despachoBarril.toFixed(2));

    $("#tbVolumenGlpBarriles").val(despachoBarril.toFixed(2));
    var volumenGlpM3 = despachoBarril * 0.1589;
    $("#tbVolumenGlpM3").val(volumenGlpM3.toFixed(2));

    var cantidadDosificadaGal = despachoGalones > 0 ? (consumoLitros / 3.785) * 8.3372 * (0.8315 / despachoGalones) * 10000 : 0;
    var cantidadDosificadaM3 = cantidadDosificadaGal / 2.2;
    $("#tbCantidadDosificadaM3").val(cantidadDosificadaM3.toFixed(2));    
    $("#tbCantidadDosificadaGal").val(cantidadDosificadaGal.toFixed(2));

    var densidadDelAgua = $("#tbDensidadDelAgua").val().length > 0 ? parseFloat($("#tbDensidadDelAgua").val()) : 0;
    var gravedadEspecificaEtil = $("#tbGravedadEspecificaEtil").val().length > 0 ? parseFloat($("#tbGravedadEspecificaEtil").val()) : 0;
    var consumoMensual = (consumoLitros / 3.785) * densidadDelAgua * gravedadEspecificaEtil;
    $("#tbConsumoMensual").val(consumoMensual.toFixed(2));

}