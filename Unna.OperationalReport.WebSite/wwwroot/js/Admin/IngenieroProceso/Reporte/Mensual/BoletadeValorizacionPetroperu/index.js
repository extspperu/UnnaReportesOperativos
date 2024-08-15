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
        Guardar();
    });

    Obtener();
}

//function descargarExcel() {
//    window.location = $("#__HD_URL_GENERAR_REPORTE").val();
//}
function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}



function Obtener() {
    $("#msjErrorObtener").hide();
    $("#loadingMensaje").show();
    $("#contenidorReporte").hide();
    $("#loadingMensaje").show();

    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000 * 20);
}

function RespuestaObtener(data) {
    console.log("dato: ", data);
    parametros = data;


    $("#contenidorReporte").show();
    $("#loadingReporte").hide();

    $("#tbPeriodo").html(data.periodo);
    $("#tbNombreReporte").html(data.nombreReporte);
    $("#tbCompania").html(data.companiaReporte);
    $("#tbVersion").html("Versión " + data.versionReporte);


    $("#tbDensidadGlp").val(data.densidadGlp);
    $("#tbMontoFacturarUnna").val(data.montoFacturarUnna.toFixed(2));
    $("#tbMontoFacturarPetroperu").val(data.montoFacturarPetroperu.toFixed(2));
    $("#tbObservacion").val(data.observacion);


    $("#GnaLoteI").val(data.gnaLoteI);
    $("#EnergiaLoteI").val(data.energiaLoteI);
    $("#LgnRecuperadosLoteI").val(data.lgnRecuperadosLoteI);
    $("#GnaLoteVi").val(data.gnaLoteVi);
    $("#EnergiaLoteVi").val(data.energiaLoteVi);
    $("#LgnRecuperadosLoteVi").val(data.lgnRecuperadosLoteVi);
    $("#GnaLoteZ69").val(data.gnaLoteZ69);
    $("#EnergiaLoteZ69").val(data.energiaLoteZ69);
    $("#LgnRecuperadosLoteZ69").val(data.lgnRecuperadosLoteZ69);
    $("#TotalGna").val(data.totalGna);
    $("#Eficiencia").val(data.eficiencia);
    $("#LiquidosRecuperados").val(data.liquidosRecuperados);
    $("#GnsLoteI").val(data.gnsLoteI);
    $("#GnsLoteVi").val(data.gnsLoteVi);
    $("#GnsLoteZ69").val(data.gnsLoteZ69);
    $("#GnsTotal").val(data.gnsTotal);
    $("#EnergiaMmbtu").val(data.energiaMmbtu);
    $("#ValorLiquidosUs").val(data.valorLiquidosUs);
    $("#CostoUnitMaquilaUsMmbtu").val(data.costoUnitMaquilaUsMmbtu);
    $("#CostoMaquilaUs").val(data.costoMaquilaUs);

    pintarValorizacionMensual(data.boletadeValorizacionPetroperu);
    pintarValorizacionMensualLotI(data.boletadeValorizacionPetroperu);
    pintarValorizacionMensualLotVi(data.boletadeValorizacionPetroperu);
    pintarValorizacionMensualLotZ69(data.boletadeValorizacionPetroperu);

    //Lote I
    $(".gnaLoteI").html(data.gnaLoteI.toFixed(2));
    $(".energiaLoteI").html(data.energiaLoteI.toFixed(4));
    $(".lgnRecuperadosLoteI").html(data.lgnRecuperadosLoteI.toFixed(2));
    $(".eficiencia").html(data.eficiencia.toFixed(2) + "%");
    $(".gnsLoteI").html(data.gnsLoteI.toFixed(4));
    $("#EnergiaMmbtuLoteI").val(data.energiaMmbtuLoteI.toFixed(2));
    $("#ValorLiquidosLoteI").val(data.valorLiquidosLoteI.toFixed(2));
    $(".costoUnitMaquilaUsMmbtu").val(data.costoUnitMaquilaUsMmbtu.toFixed(4));
    $("#CostoMaquillaLoteI").val(data.costoMaquillaLoteI.toFixed(2));
    $(".densidadGlp").val(data.densidadGlp);
    $("#MontoFacturarLoteI").val(data.montoFacturarLoteI);
    $("#ValorLiquidosLoteIFinal").val(data.valorLiquidosLoteI);
    $("#ObservacionLoteI").val(data.observacionLoteI);
    //Lote Vi
    $(".gnaLoteVi").html(data.gnaLoteVi.toFixed(2));
    $(".energiaLoteVi").html(data.energiaLoteVi.toFixed(4));
    $(".lgnRecuperadosLoteVi").html(data.lgnRecuperadosLoteVi.toFixed(2));
    $(".eficiencia").html(data.eficiencia.toFixed(2) + "%");
    $(".gnsLoteVi").html(data.gnsLoteVi.toFixed(4));
    $("#EnergiaMmbtuLoteVi").val(data.energiaMmbtuLoteVi.toFixed(2));
    $("#ValorLiquidosLoteVi").val(data.valorLiquidosLoteVi.toFixed(2));
    $(".costoUnitMaquilaUsMmbtu").val(data.costoUnitMaquilaUsMmbtu.toFixed(4));
    $("#CostoMaquillaLoteVi").val(data.costoMaquillaLoteVi.toFixed(2));
    $(".densidadGlp").val(data.densidadGlp);
    $("#MontoFacturarLoteVi").val(data.montoFacturarLoteVi);
    $("#ValorLiquidosLoteViFinal").val(data.valorLiquidosLoteVi.toFixed(2));
    $("#ObservacionLoteVi").val(data.observacionLoteVi);

    //Lote Z69
    $(".gnaLoteZ69").html(data.gnaLoteZ69.toFixed(2));
    $(".energiaLoteZ69").html(data.energiaLoteZ69.toFixed(4));
    $(".lgnRecuperadosLoteZ69").html(data.lgnRecuperadosLoteZ69.toFixed(2));
    $(".eficiencia").html(data.eficiencia.toFixed(2) + "%");
    $(".gnsLoteZ69").html(data.gnsLoteZ69.toFixed(4));
    $("#EnergiaMmbtuLoteZ69").val(data.energiaMmbtuLoteZ69.toFixed(2));
    $("#ValorLiquidosLoteZ69").val(data.valorLiquidosLoteZ69.toFixed(2));
    $(".costoUnitMaquilaUsMmbtu").val(data.costoUnitMaquilaUsMmbtu.toFixed(4));
    $("#CostoMaquillaLoteZ69").val(data.costoMaquillaLoteZ69.toFixed(2));
    $(".densidadGlp").val(data.densidadGlp);
    $("#MontoFacturarLoteZ69").val(data.montoFacturarLoteZ69);
    $("#ValorLiquidosLoteZ69Final").val(data.valorLiquidosLoteZ69.toFixed(2));
    $("#ObservacionLoteZ69").val(data.observacionLoteZ69);

}

function ObtenerError(data) {
    console.log(data);
    $("#msjErrorObtener").show();
    $("#loadingMensaje").hide();
    $("#contenidorReporte").hide();
    $("#loadingMensaje").hide();
}

function pintarValorizacionMensual(data) {

    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].dia + '">' +
            '<td>' + data[i].dia + '</td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnaLoteI_' + data[i].dia + '" value="' + data[i].gnaLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="poderCalorificoLoteI_' + data[i].dia + '" value="' + data[i].poderCalorificoLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="energiaLoteI_' + data[i].dia + '" value="' + data[i].energiaLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaLoteI_' + data[i].dia + '" value="' + data[i].riquezaLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaBlLoteI_' + data[i].dia + '" value="' + data[i].riquezaBlLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="lgnRecuperadosLoteI_' + data[i].dia + '" value="' + data[i].lgnRecuperadosLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnaLoteVi_' + data[i].dia + '" value="' + data[i].gnaLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="poderCalorificoLoteVi_' + data[i].dia + '" value="' + data[i].poderCalorificoLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="energiaLoteVi_' + data[i].dia + '" value="' + data[i].energiaLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaLoteVi_' + data[i].dia + '" value="' + data[i].riquezaLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaBlLoteVi_' + data[i].dia + '" value="' + data[i].riquezaBlLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="lgnRecuperadosLoteVi_' + data[i].dia + '" value="' + data[i].lgnRecuperadosLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnaLoteZ69_' + data[i].dia + '" value="' + data[i].gnaLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="poderCalorificoLoteZ69_' + data[i].dia + '" value="' + data[i].poderCalorificoLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="energiaLoteZ69_' + data[i].dia + '" value="' + data[i].energiaLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaLoteZ69_' + data[i].dia + '" value="' + data[i].riquezaLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="riquezaBlLoteZ69_' + data[i].dia + '" value="' + data[i].riquezaBlLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="lgnRecuperadosLoteZ69_' + data[i].dia + '" value="' + data[i].lgnRecuperadosLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="totalGna_' + data[i].dia + '" value="' + data[i].totalGna + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="eficiencia_' + data[i].dia + '" value="' + data[i].eficiencia + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="totalLiquidosRecuperados_' + data[i].dia + '" value="' + data[i].totalLiquidosRecuperados + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnsLoteI_' + data[i].dia + '" value="' + data[i].gnsLoteI + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnsLoteVi_' + data[i].dia + '" value="' + data[i].gnsLoteVi + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnsLoteZ69_' + data[i].dia + '" value="' + data[i].gnsLoteZ69 + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="gnsTotal_' + data[i].dia + '" value="' + data[i].gnsTotal + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="poderCalorificoBtuPcsd_' + data[i].dia + '" value="' + data[i].poderCalorificoBtuPcsd + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="energiaMmbtu_' + data[i].dia + '" value="' + data[i].energiaMmbtu + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="precioGlpESinIgvSolesKg_' + data[i].dia + '" value="' + data[i].precioGlpESinIgvSolesKg + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="precioGlpGSinIGVSolesKg_' + data[i].dia + '" value="' + data[i].precioGlpGSinIGVSolesKg + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="precioRefGlpSinIgvSolesKg_' + data[i].dia + '" value="' + data[i].precioRefGlpSinIgvSolesKg + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="precioGLPSinIgvUsBl_' + data[i].dia + '" value="' + data[i].precioGLPSinIgvUsBl + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tipodeCambioSolesUs_' + data[i].dia + '" value="' + data[i].tipodeCambioSolesUs + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="precioCgnUsBl_' + data[i].dia + '" value="' + data[i].precioCgnUsBl + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="valorLiquidosUs_' + data[i].dia + '" value="' + data[i].valorLiquidosUs + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="costoUnitMaquilaUsMmbtu_' + data[i].dia + '" value="' + data[i].costoUnitMaquilaUsMmbtu + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="costoMaquilaUs_' + data[i].dia + '" value="' + data[i].costoMaquilaUs + '"></td>' +
            '</tr>';
    }
    $("#tblAnexo7ValorizacionMensual tbody").html(html);
}



function pintarValorizacionMensualLotI(data) {
    console.log("data: ", data);
    var html = "";
    if (data.length == 0) {
        html += '<tr>' +
            '<td colspan="19">No existe registros</td>' +
            '</tr>';
    } else {
        for (var i = 0; i < data.length; i++) {
            html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].dia + '">' +
                '<td><p class="mb-0 text-right">' + data[i].dia + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnaLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaBlLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].lgnRecuperadosLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].eficiencia + '%</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnsLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoBtuPcsd + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaMmbtuLoteI + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpESinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpGSinIGVSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioRefGlpSinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGLPSinIgvUsBl + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].tipodeCambioSolesUs + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioCgnUsBl.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].valorLiquidosLotI.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoUnitMaquilaUsMmbtu.toFixed(4) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoMaquilaUsLotI.toFixed(2) + '</p></td>' +
                '</tr>';
        }
    }
    $("#tblAnexo7ValorizacionMensualLoteI tbody").html(html);
}

function pintarValorizacionMensualLotVi(data) {
    var html = "";
    if (data.length == 0) {
        html += '<tr>' +
            '<td colspan="19">No existe registros</td>' +
            '</tr>';
    } else {
        for (var i = 0; i < data.length; i++) {
            html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].dia + '">' +
                '<td><p class="mb-0 text-right">' + data[i].dia + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnaLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaBlLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].lgnRecuperadosLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].eficiencia + '%</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnsLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoBtuPcsd + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaMmbtuLoteVi + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpESinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpGSinIGVSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioRefGlpSinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGLPSinIgvUsBl + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].tipodeCambioSolesUs + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioCgnUsBl.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].valorLiquidosLotVi.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoUnitMaquilaUsMmbtu.toFixed(4) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoMaquilaUsLotVi.toFixed(2) + '</p></td>' +
                '</tr>';
        }
    }
    $("#tblAnexo7ValorizacionMensualLoteVi tbody").html(html);
}

function pintarValorizacionMensualLotZ69(data) {
    var html = "";
    if (data.length == 0) {
        html += '<tr>' +
            '<td colspan="19">No existe registros</td>' +
            '</tr>';
    } else {
        for (var i = 0; i < data.length; i++) {
            html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].dia + '">' +
                '<td><p class="mb-0 text-right">' + data[i].dia + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnaLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].riquezaBlLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].lgnRecuperadosLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].eficiencia + '%</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].gnsLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].poderCalorificoBtuPcsd + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].energiaMmbtuLoteZ69 + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpESinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGlpGSinIGVSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioRefGlpSinIgvSolesKg + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioGLPSinIgvUsBl + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].tipodeCambioSolesUs + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].precioCgnUsBl.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].valorLiquidosLotZ69.toFixed(2) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoUnitMaquilaUsMmbtu.toFixed(4) + '</p></td>' +
                '<td><p class="mb-0 text-right">' + data[i].costoMaquilaUsLotZ69.toFixed(2) + '</p></td>' +
                '</tr>';
        }
    }
    $("#tblAnexo7ValorizacionMensualLoteZ69 tbody").html(html);
}

function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    $("#btnGuardar").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnGuardar").prop("disabled", true);
    var url = $('#__URL_GUARDAR_REPORTE').val();
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.boletadeValorizacionPetroperu.length; i++) {
            if (parametros.boletadeValorizacionPetroperu[i].dia == datoId) {
                parametros.boletadeValorizacionPetroperu[i].gnaLoteI = $("#gnaLoteI_" + datoId).val().length > 0 ? $("#gnaLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].poderCalorificoLoteI = $("#poderCalorificoLoteI_" + datoId).val().length > 0 ? $("#poderCalorificoLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].energiaLoteI = $("#energiaLoteI_" + datoId).val().length > 0 ? $("#energiaLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].riquezaLoteI = $("#riquezaLoteI_" + datoId).val().length > 0 ? $("#riquezaLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].lgnRecuperadosLoteI = $("#lgnRecuperadosLoteI_" + datoId).val().length > 0 ? $("#lgnRecuperadosLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnaLoteVi = $("#gnaLoteVi_" + datoId).val().length > 0 ? $("#gnaLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].poderCalorificoLoteVi = $("#poderCalorificoLoteVi_" + datoId).val().length > 0 ? $("#poderCalorificoLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].energiaLoteVi = $("#energiaLoteVi_" + datoId).val().length > 0 ? $("#energiaLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].riquezaLoteVi = $("#riquezaLoteVi_" + datoId).val().length > 0 ? $("#riquezaLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].riquezaBlLoteVi = $("#riquezaBlLoteVi_" + datoId).val().length > 0 ? $("#riquezaBlLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].lgnRecuperadosLoteVi = $("#lgnRecuperadosLoteVi_" + datoId).val().length > 0 ? $("#lgnRecuperadosLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnaLoteZ69 = $("#gnaLoteZ69_" + datoId).val().length > 0 ? $("#gnaLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].poderCalorificoLoteZ69 = $("#poderCalorificoLoteZ69_" + datoId).val().length > 0 ? $("#poderCalorificoLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].energiaLoteZ69 = $("#energiaLoteZ69_" + datoId).val().length > 0 ? $("#energiaLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].riquezaLoteZ69 = $("#riquezaLoteZ69_" + datoId).val().length > 0 ? $("#riquezaLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].riquezaBlLoteZ69 = $("#riquezaBlLoteZ69_" + datoId).val().length > 0 ? $("#riquezaBlLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].lgnRecuperadosLoteZ69 = $("#lgnRecuperadosLoteZ69_" + datoId).val().length > 0 ? $("#lgnRecuperadosLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].totalGna = $("#totalGna_" + datoId).val().length > 0 ? $("#totalGna_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].eficiencia = $("#eficiencia_" + datoId).val().length > 0 ? $("#eficiencia_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].totalLiquidosRecuperados = $("#totalLiquidosRecuperados_" + datoId).val().length > 0 ? $("#totalLiquidosRecuperados_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnsLoteI = $("#gnsLoteI_" + datoId).val().length > 0 ? $("#gnsLoteI_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnsLoteVi = $("#gnsLoteVi_" + datoId).val().length > 0 ? $("#gnsLoteVi_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnsLoteZ69 = $("#gnsLoteZ69_" + datoId).val().length > 0 ? $("#gnsLoteZ69_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].gnsTotal = $("#gnsTotal_" + datoId).val().length > 0 ? $("#gnsTotal_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].poderCalorificoBtuPcsd = $("#poderCalorificoBtuPcsd_" + datoId).val().length > 0 ? $("#poderCalorificoBtuPcsd_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].energiaMmbtu = $("#energiaMmbtu_" + datoId).val().length > 0 ? $("#energiaMmbtu_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].precioGlpESinIgvSolesKg = $("#precioGlpESinIgvSolesKg_" + datoId).val().length > 0 ? $("#precioGlpESinIgvSolesKg_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].precioGlpGSinIGVSolesKg = $("#precioGlpGSinIGVSolesKg_" + datoId).val().length > 0 ? $("#precioGlpGSinIGVSolesKg_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].precioRefGlpSinIgvSolesKg = $("#precioRefGlpSinIgvSolesKg_" + datoId).val().length > 0 ? $("#precioRefGlpSinIgvSolesKg_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].precioGLPSinIgvUsBl = $("#precioGLPSinIgvUsBl_" + datoId).val().length > 0 ? $("#precioGLPSinIgvUsBl_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].tipodeCambioSolesUs = $("#tipodeCambioSolesUs_" + datoId).val().length > 0 ? $("#tipodeCambioSolesUs_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].precioCgnUsBl = $("#precioCgnUsBl_" + datoId).val().length > 0 ? $("#precioCgnUsBl_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].valorLiquidosUs = $("#valorLiquidosUs_" + datoId).val().length > 0 ? $("#valorLiquidosUs_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].costoUnitMaquilaUsMmbtu = $("#costoUnitMaquilaUsMmbtu_" + datoId).val().length > 0 ? $("#costoUnitMaquilaUsMmbtu_" + datoId).val() : null;
                parametros.boletadeValorizacionPetroperu[i].costoMaquilaUs = $("#costoMaquilaUs_" + datoId).val().length > 0 ? $("#costoMaquilaUs_" + datoId).val() : null;
            }

        }
    });

    parametros.gnaLoteI = $("#GnaLoteI").val();
    parametros.energiaLoteI = $("#EnergiaLoteI").val();
    parametros.lgnRecuperadosLoteI = $("#LgnRecuperadosLoteI").val();
    parametros.gnaLoteVi = $("#GnaLoteVi").val();
    parametros.energiaLoteVi = $("#EnergiaLoteVi").val();
    parametros.lgnRecuperadosLoteVi = $("#LgnRecuperadosLoteVi").val();
    parametros.gnaLoteZ69 = $("#GnaLoteZ69").val();
    parametros.energiaLoteZ69 = $("#EnergiaLoteZ69").val();
    parametros.lgnRecuperadosLoteZ69 = $("#LgnRecuperadosLoteZ69").val();
    parametros.totalGna = $("#TotalGna").val();
    parametros.eficiencia = $("#Eficiencia").val();
    parametros.liquidosRecuperados = $("#LiquidosRecuperados").val();
    parametros.gnsLoteI = $("#GnsLoteI").val();
    parametros.gnsLoteVi = $("#GnsLoteVi").val();
    parametros.gnsLoteZ69 = $("#GnsLoteZ69").val();
    parametros.gnsTotal = $("#GnsTotal").val();
    parametros.energiaMmbtu = $("#EnergiaMmbtu").val();
    parametros.valorLiquidosUs = $("#ValorLiquidosUs").val();
    parametros.costoUnitMaquilaUsMmbtu = $("#CostoUnitMaquilaUsMmbtu").val();
    parametros.costoMaquilaUs = $("#CostoMaquilaUs").val();

    parametros.observacion = $("#tbObservacion").val();
    parametros.densidadGlp = $("#tbDensidadGlp").val();
    parametros.montoFacturarUnna = $("#tbMontoFacturarUnna").val();
    parametros.montoFacturarPetroperu = $("#tbMontoFacturarPetroperu").val();


    parametros.energiaMmbtuLoteI = $("#EnergiaMmbtuLoteI").val();
    parametros.valorLiquidosLoteI = $("#ValorLiquidosLoteI").val();
    parametros.costoMaquillaLoteI = $("#CostoMaquillaLoteI").val();
    parametros.montoFacturarLoteI = $("#MontoFacturarLoteI").val();
    parametros.observacionLoteI = $("#ObservacionLoteI").val();

    parametros.energiaMmbtuLoteVi = $("#EnergiaMmbtuLoteVi").val();
    parametros.valorLiquidosLoteVi = $("#ValorLiquidosLoteVi").val();
    parametros.costoMaquillaLoteVi = $("#CostoMaquillaLoteVi").val();
    parametros.montoFacturarLoteVi = $("#MontoFacturarLoteVi").val();
    parametros.observacionLoteVi = $("#ObservacionLoteVi").val();

    parametros.energiaMmbtuLoteZ69 = $("#EnergiaMmbtuLoteZ69").val();
    parametros.valorLiquidosLoteZ69 = $("#ValorLiquidosLoteZ69").val();
    parametros.costoMaquillaLoteZ69 = $("#CostoMaquillaLoteZ69").val();
    parametros.montoFacturarLoteZ69 = $("#MontoFacturarLoteZ69").val();
    parametros.observacionLoteZ69 = $("#ObservacionLoteZ69").val();
    
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);

}