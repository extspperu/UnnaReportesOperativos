
var parametros;
$(document).ready(function () {
    controles();
});

function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnDescargarPdf').click(function () {
        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
}

function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}


function Obtener() {
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
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
    MensajeAlerta("No se pudo completar el registro", "error");
}

function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();

    $('.list-datos-tblGasNaturalAsociado').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.gasNaturalAsociado.length; i++) {
            if (parametros.gasNaturalAsociado[i].idLote == datoId || parametros.gasNaturalAsociado[i].idLote == null) {
                parametros.gasNaturalAsociado[i].volumen = $("#tbVolumentbl1_" + datoId).val().length > 0 ? $("#tbVolumentbl1_" + datoId).val() : null;
                parametros.gasNaturalAsociado[i].calorifico = $("#tbCalorificotbl1_" + datoId).val().length > 0 ? $("#tbCalorificotbl1_" + datoId).val() : null;
                parametros.gasNaturalAsociado[i].riqueza = $("#tbRiquezatbl1_" + datoId).val().length > 0 ? $("#tbRiquezatbl1_" + datoId).val() : null;
                parametros.gasNaturalAsociado[i].riquezaBls = $("#tbRiquezaBlstbl1_" + datoId).val().length > 0 ? $("#tbRiquezaBlstbl1_" + datoId).val() : null;
                parametros.gasNaturalAsociado[i].energiaDiaria = $("#tbEnergiaDiariatbl1_" + datoId).val().length > 0 ? $("#tbEnergiaDiariatbl1_" + datoId).val() : null;
                parametros.gasNaturalAsociado[i].volumenPromedio = $("#tbVolumenPromediobl1_" + datoId).val().length > 0 ? $("#tbVolumenPromediobl1_" + datoId).val() : null;
            }
        }
    });

    parametros.gasProcesado = $("#tbGasProcesado").val();
    parametros.gasNoProcesado = $("#tbGasNoProcesado").val();
    parametros.utilizacionPlantaParinias = $("#tbUtilizacionPlantaParinias").val();
    parametros.horaPlantaFs = $("#tbHoraPlantaFs").val();

    parametros.eficienciaRecuperacionLgn = $("#tbEficienciaRecuperacionLgn").val();

    $('.list-datos-tblGasNaturalSeco').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.gasNaturalSeco.length; i++) {
            if (parametros.gasNaturalSeco[i].item == datoId || parametros.gasNaturalSeco[i].item == null) {
                parametros.gasNaturalSeco[i].volumen = $("#tbVolumenTbl2_" + datoId).val().length > 0 ? $("#tbVolumenTbl2_" + datoId).val() : null;
                parametros.gasNaturalSeco[i].calorifico = $("#tbCalorificoTbl2_" + datoId).val().length > 0 ? $("#tbCalorificoTbl2_" + datoId).val() : null;
                parametros.gasNaturalSeco[i].volumenPromedio = $("#tbVolumenPromedioTbl2_" + datoId).val().length > 0 ? $("#tbVolumenPromedioTbl2_" + datoId).val() : null;
                parametros.gasNaturalSeco[i].energiaDiaria = $("#tbEnergiaDiariaTbl2_" + datoId).val().length > 0 ? $("#tbEnergiaDiariaTbl2_" + datoId).val() : null;
            }
        }
    });



    $('.list-datos-tblLiquidosGasNaturalProduccionVentas').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.liquidosGasNaturalProduccionVentas.length; i++) {
            if (parametros.liquidosGasNaturalProduccionVentas[i].producto == datoId || parametros.liquidosGasNaturalProduccionVentas[i].producto == null) {
                parametros.liquidosGasNaturalProduccionVentas[i].produccionDiaria = $("#tbProduccionDiariaTbl3_" + datoId).val().length > 0 ? $("#tbProduccionDiariaTbl3_" + datoId).val() : null;
                parametros.liquidosGasNaturalProduccionVentas[i].produccionMensual = $("#tbProduccionMensualTbl3_" + datoId).val().length > 0 ? $("#tbProduccionMensualTbl3_" + datoId).val() : null;
                parametros.liquidosGasNaturalProduccionVentas[i].ventaDiaria = $("#tbVentaDiariaTbl3_" + datoId).val().length > 0 ? $("#tbVentaDiariaTbl3_" + datoId).val() : null;
                parametros.liquidosGasNaturalProduccionVentas[i].ventaMensual = $("#tbVentaMensualTbl3_" + datoId).val().length > 0 ? $("#tbVentaMensualTbl3_" + datoId).val() : null;
            }
        }
    });


    $('.list-datos-tbl-volumenProduccionLoteXGnaTotalCnpc').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLoteXGnaTotalCnpc.length; i++) {
            if (parametros.volumenProduccionLoteXGnaTotalCnpc[i].item == datoId || parametros.volumenProduccionLoteXGnaTotalCnpc[i].item == null) {
                parametros.volumenProduccionLoteXGnaTotalCnpc[i].volumen = $("#txtVolumenProduccionLoteXGnaTotalCnpc_" + datoId).val().length > 0 ? $("#txtVolumenProduccionLoteXGnaTotalCnpc_" + datoId).val() : null;
            }
        }
    });
    $('.list-datos-tbl-volumenProduccionLoteXLiquidoGasNatural').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLoteXLiquidoGasNatural.length; i++) {
            if (parametros.volumenProduccionLoteXLiquidoGasNatural[i].item == datoId || parametros.volumenProduccionLoteXLiquidoGasNatural[i].item == null) {
                parametros.volumenProduccionLoteXLiquidoGasNatural[i].volumen = $("#txtVolumenProduccionLoteXLiquidoGasNatural_" + datoId).val().length > 0 ? $("#txtVolumenProduccionLoteXLiquidoGasNatural_" + datoId).val() : null;
            }
        }
    });
    $('.list-datos-tbl-volumenProduccionEnel').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionEnel.length; i++) {
            if (parametros.volumenProduccionEnel[i].item == datoId || parametros.volumenProduccionEnel[i].item == null) {
                parametros.volumenProduccionEnel[i].volumen = $("#txtVolumenProduccionEnel_" + datoId).val().length > 0 ? $("#txtVolumenProduccionEnel_" + datoId).val() : null;
            }
        }
    });
    $('.list-datos-tbl-volumenProduccionGasNaturalEnel').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionGasNaturalEnel.length; i++) {
            if (parametros.volumenProduccionGasNaturalEnel[i].item == datoId || parametros.volumenProduccionGasNaturalEnel[i].item == null) {
                parametros.volumenProduccionGasNaturalEnel[i].volumen = $("#txtVolumenLiquidoGasTbl5_" + datoId).val().length > 0 ? $("#txtVolumenLiquidoGasTbl5_" + datoId).val() : null;
            }
        }
    });

    $('.list-datos-tbl-volumenProduccionPetroperu').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLiquidoGasNatural.length; i++) {
            if (parametros.volumenProduccionLiquidoGasNatural[i].item == datoId || parametros.volumenProduccionLiquidoGasNatural[i].item == null) {
                parametros.volumenProduccionLiquidoGasNatural[i].gnaRecibido = $("#txtGnsTrasferidoPetroPeru_" + datoId).val().length > 0 ? $("#txtGnsTrasferidoPetroPeru_" + datoId).val() : null;
                parametros.volumenProduccionLiquidoGasNatural[i].gnsTrasferido = $("#txtGnsTrasferidoPetroPeru_" + datoId).val().length > 0 ? $("#txtGnsTrasferidoPetroPeru_" + datoId).val() : null;
            }
        }
    });


    $('.list-datos-tbl-volumenProduccionLiquidoGasNatural').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLiquidoGasNatural.length; i++) {
            if (parametros.volumenProduccionLiquidoGasNatural[i].item == datoId || parametros.volumenProduccionLiquidoGasNatural[i].item == null) {
                parametros.volumenProduccionLiquidoGasNatural[i].loteZ69 = $("#txtLoteZ69Tbl6_" + datoId).val().length > 0 ? $("#txtLoteZ69Tbl6_" + datoId).val() : null;
                parametros.volumenProduccionLiquidoGasNatural[i].loteVi = $("#txtLoteViTbl6_" + datoId).val().length > 0 ? $("#txtLoteViTbl6_" + datoId).val() : null;
                parametros.volumenProduccionLiquidoGasNatural[i].loteI = $("#txtLoteITbl6_" + datoId).val().length > 0 ? $("#txtLoteITbl6_" + datoId).val() : null;
            }
        }
    });
    parametros.gasAlfare = $("#tbGasAlfare").val();
    $('.list-datos-tbl-volumenProduccionLoteIvUnnaEnegia').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLoteIvUnnaEnegia.length; i++) {
            if (parametros.volumenProduccionLoteIvUnnaEnegia[i].item == datoId || parametros.volumenProduccionLoteIvUnnaEnegia[i].item == null) {
                parametros.volumenProduccionLoteIvUnnaEnegia[i].volumen = $("#txtVolumenTbl7_" + datoId).val().length > 0 ? $("#txtVolumenTbl7_" + datoId).val() : null;
            }
        }
    });
  
    $('.list-datos-tbl-volumenProduccionLoteIvLiquidoGasNatural').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenProduccionLoteIvLiquidoGasNatural.length; i++) {
            if (parametros.volumenProduccionLoteIvLiquidoGasNatural[i].item == datoId || parametros.volumenProduccionLoteIvLiquidoGasNatural[i].item == null) {
                parametros.volumenProduccionLoteIvLiquidoGasNatural[i].volumen = $("#tbVolumenLoteIv_" + datoId).val().length > 0 ? $("#tbVolumenLoteIv_" + datoId).val() : null;                
            }
        }
    });

    parametros.comentario = $("#tbComentario").val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = jqXHR.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}