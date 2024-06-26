
var parametros = null;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnGuardar').click(function () {
        Guardar();
    });
    $('#btnCargarReporte').change(function () {
        CargarExcelBase();
    });
    Obtener();
}

function descargarExcel() {
    window.location = $("#__HD_URL_GENERAR_REPORTE_EXCEL").val();
}



function Obtener() {
    
    $("#contenidoResultado").hide();
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
    $("#contenidoResultado").show();
    parametros = data;
    $("#tbNombreReporte").html(data.nombreReporte);
    $("#tbPeriodo").html(data.periodo);
    $("#txtTotalVolumen").val(data.totalVolumen);
    $("#txtTotalPoderCalorifico").val(data.totalPoderCalorifico);
    $("#txtTotalEnergia").val(data.totalEnergia);

    $("#txtEnergiaVolumenSuministrado").val(data.energiaVolumenSuministrado);
    $("#txtPrecioBaseUsd").val(data.precioBase);
    $("#txtFac").val(data.fac);
    $("#txtSubTotal").val(data.subTotal);
    $("#txtIgv").val(data.igv);
    $("#txtIgvCentaje").html('IGV ' + data.igvCentaje + '%');
    $("#txtCPIo").val(data.cpIo);
    $("#txtCPIi").val(data.cpIi);
    $("#txtTotalFacturar").val(data.total);
    $("#txtComentario").val(data.comentario);
    if (data.boletaVentaMenensual != null) {
        LlenarTablaBoletaVenta(data.boletaVentaMenensual);
    }
    if (data.urlFirma != null) {
        $("#tbUrlFirma").attr("src", data.urlFirma);
    }
    

}


function ObtenerError(data) {
    console.log(data);
}

function LlenarTablaBoletaVenta(data) {
    
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].id + '">' +
            '<td>' + data[i].fecha + '</td>' +
            '<td>' + data[i].placa + '</td>' +
            '<td>' + data[i].fechaInicioCarga + '</td>' +
            '<td>' + data[i].fechaFinCarga + '</td>' +
            '<td>' + data[i].nroConstanciaDespacho + '</td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbVolumen_' + data[i].id + '" value="' + data[i].volumen + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbPoderCalorifico_' + data[i].id + '" value="' + data[i].poderCalorifico + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbEnergia_' + data[i].id + '" value="' + data[i].energia + '"></td>' +
            '</tr>';
    }
    $("#tableBodyRegistros tbody").html(html);
    
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
        for (var i = 0; i < parametros.boletaVentaMenensual.length; i++) {
            if (parametros.boletaVentaMenensual[i].id == datoId) {
                parametros.boletaVentaMenensual[i].volumen = $("#tbVolumen_" + datoId).val().length > 0 ? $("#tbVolumen_" + datoId).val() : null;
                parametros.boletaVentaMenensual[i].poderCalorifico = $("#tbPoderCalorifico_" + datoId).val().length > 0 ? $("#tbPoderCalorifico_" + datoId).val() : null;
                parametros.boletaVentaMenensual[i].energia = $("#tbEnergia_" + datoId).val().length > 0 ? $("#tbEnergia_" + datoId).val() : null;
            }

        }
    });
    parametros.energiaVolumenSuministrado = $("#txtEnergiaVolumenSuministrado").val();
    parametros.precioBase = $("#txtPrecioBaseUsd").val();
    parametros.fac = $("#txtFac").val();
    parametros.igv = $("#txtIgv").val();
    parametros.subTotal = $("#txtTotalFacturar").val();
    parametros.cpIo = $("#txtCPIo").val();
    parametros.cpIi = $("#txtCPIi").val();
    parametros.total = $("#txtTotalFacturar").val();
    parametros.comentario = $("#txtComentario").val();

    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);

}



function CargarExcelBase() {
    $("#btnCargarReporteLabel").prop("disabled", true);
    $("#btnCargarReporteLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_SUBIR_ARCHIVO_REPORTE').val();
    var dato = new FormData($("#FormCargarReporte")[0]);
    console.log(dato);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnCargarReporteLabel").html("Cargar Excel");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
            MensajeAlerta("Se proceso correctamente", "success");    
            Obtener();
        },
        error: function (jqXHR, status, error) {
            console.log(jqXHR);
            console.log(status);
            MensajeAlerta("No se puede completar la carga, intente nuevamente", "error");
            $("#btnCargarReporteLabel").html("Cargar Excel");
            $("#btnCargarReporteLabel").prop("disabled", false);
            document.getElementById("btnCargarReporte").value = null;
        },
    });
}