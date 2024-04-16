
var parametros;
$(document).ready(function () {
    controles();
    
});


function controles() {
   
    $('#btnDescargarExcel').click(function () {
        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
    
}


function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}


function Obtener() {
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log("dato: ", data);
    parametros = data;
    Guardar();
}

function ObtenerError(data) {
    console.log(data);
}

function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();
    parametros.gnaEntregaUnna.volumen = $("#tbVolumen").val();
    parametros.gnaEntregaUnna.poderCalorifico = $("#tbPoderCalorifico").val();
    parametros.gnaEntregaUnna.energia = $("#tbEnergia").val();
    parametros.gnaEntregaUnna.riqueza = $("#tbRiqueza").val();

    parametros.comPesadosGna = $("#tbComPesadosGna").val();
    parametros.porcentajeEficiencia = $("#tbPorcentajeEficiencia").val();
    parametros.contenidoCalorificoPromLgn = $("#tbContenidoCalorificoPromLgn").val();

    $('.list-datos-tblLiquidosBarriles').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.liquidosBarriles.length; i++) {
            if (parametros.liquidosBarriles[i].id == datoId || parametros.liquidosBarriles[i].id == null) {
                parametros.liquidosBarriles[i].volumen = $("#tbEnel_" + datoId).val().length > 0 ? $("#tbEnel_" + datoId).val() : null;
                parametros.liquidosBarriles[i].concentracionC1 = $("#tbBlsd_" + datoId).val().length > 0 ? $("#tbBlsd_" + datoId).val() : null;                
            }
        }
    });

    $('.list-datos-tblGnsAEnel').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.gnsAEnel.length; i++) {
            if (parametros.gnsAEnel[i].item == datoId || parametros.gnsAEnel[i].item == null) {
                parametros.gnsAEnel[i].volumen = $("#tbGnsAEnelVolumen_" + datoId).val().length > 0 ? $("#tbGnsAEnelVolumen_" + datoId).val() : null;
                parametros.gnsAEnel[i].concentracionC1 = $("#tbGnsAEnelPoderCalorifico_" + datoId).val().length > 0 ? $("#tbGnsAEnelPoderCalorifico_" + datoId).val() : null;
                parametros.gnsAEnel[i].volumenC1 = $("#tbGnsAEnelEnergia_" + datoId).val().length > 0 ? $("#tbGnsAEnelEnergia_" + datoId).val() : null;                
            }
        }
    });

    parametros.volumenProduccionTotalGlp = $("#VolumenProduccionTotalGlp").val();
    parametros.volumenProduccionTotalCgn = $("#VolumenProduccionTotalCgn").val();
    parametros.volumenProduccionTotalLgn = $("#VolumenProduccionTotalLgn").val();
    $('.list-datos-tablaFDLGN').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factoresDistribucionLiquidoGasNatural.length; i++) {
            if (parametros.factoresDistribucionLiquidoGasNatural[i].item == datoId || parametros.factoresDistribucionLiquidoGasNatural[i].item == null) {
                parametros.factoresDistribucionLiquidoGasNatural[i].volumen = $("#FDLGNVolumen_" + datoId).val().length > 0 ? $("#FDLGNVolumen_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].riqueza = $("#FDLGNRiqueza_" + datoId).val().length > 0 ? $("#FDLGNRiqueza_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].contenido = $("#FDLGNContenido_" + datoId).val().length > 0 ? $("#FDLGNContenido_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].factoresDistribucion = $("#FDLGNFactoresDistribucion_" + datoId).val().length > 0 ? $("#FDLGNFactoresDistribucion_" + datoId).val() : null;
                parametros.factoresDistribucionLiquidoGasNatural[i].asignacionGns = $("#FDLGNAsignacionGns_" + datoId).val().length > 0 ? $("#FDLGNAsignacionGns_" + datoId).val() : null;
            }
        }
    });

    parametros.volumenTotalGasCombustible = $("#VolumenTotalGasCombustible").val();

    parametros.gravedadEspecifica = $("#GravedadEspecifica").val();
    parametros.volumenProduccionTotalGlpCnpc = $("#VolumenProduccionTotalGlpCnpc").val();
    parametros.volumenProduccionTotalCgnCnpc = $("#VolumenProduccionTotalCgnCnpc").val();
    parametros.comentario = $("#tbComentario").val();    
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}