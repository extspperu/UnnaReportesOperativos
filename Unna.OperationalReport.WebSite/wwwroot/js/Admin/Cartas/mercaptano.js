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

}

function cargarOsinergmin1(data) {

    // Asignar Periodo
    $("#periodoSH1").text(data.periodo);
    $("#periodoSH2").text(data.periodo);

    // Asignar valores a la tabla 1: Recepción de Gas Natural Asociado
    $("#recepcionLoteZ2B").val(data.recepcionGasNaturalAsociado.loteZ69);
    $("#recepcionLoteX").val(data.recepcionGasNaturalAsociado.loteX);
    $("#recepcionLoteVI").val(data.recepcionGasNaturalAsociado.loteVI);
    $("#recepcionLoteI").val(data.recepcionGasNaturalAsociado.loteI);
    $("#recepcionLoteV").val(data.recepcionGasNaturalAsociado.loteIV);
    $("#recepcionTotal").val(data.recepcionGasNaturalAsociado.total);

    // Asignar valores a la tabla 2: Usos del Gas
    $("#usoGasNaturalRestituido").val(data.usoGas.gasNaturalRestituido);
    $("#usoConsumoPropio").val(data.usoGas.consumoPropio);
    $("#usoConvertidoEnLgn").val(data.usoGas.convertidoEnLgn);
    $("#usoTotal").val(data.usoGas.total);

    // Asignar valores a la tabla 3: Producción de Líquidos del Gas Natural
    $("#produccionGlp").val(data.produccionLiquidosGasNatural.glp);
    $("#produccionPropanoSaturado").val(data.produccionLiquidosGasNatural.propanoSaturado);
    $("#produccionButanoSaturado").val(data.produccionLiquidosGasNatural.butanoSaturado);
    $("#produccionHexano").val(data.produccionLiquidosGasNatural.hexano);
    $("#produccionCondensados").val(data.produccionLiquidosGasNatural.condensados);
    $("#produccionPromedioLiquidos").val(data.produccionLiquidosGasNatural.promedioLiquidos);
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
