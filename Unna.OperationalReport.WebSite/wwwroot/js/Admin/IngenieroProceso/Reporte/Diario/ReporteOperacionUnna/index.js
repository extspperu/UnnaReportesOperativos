var parametros;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
}

function descargarExcel() {
    $("#btnDescargarExcel").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnDescargarExcel").prop("disabled", true);
    window.location = $("#__HD_URL_GENERAR_REPORTE").val();
    $("#btnDescargarExcel").html('Descargar');
    $("#btnDescargarExcel").prop("disabled", false);
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
    parametros = data;
    Guardar();
}

function ObtenerError(data) {
    console.log(data);
    MensajeAlerta("No se pudo completar el registro", "error");
}



function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    console.log("parametros: ", parametros);
    parametros.capacidadDisenio = $("#CapacidadDisenio").val();

    if (parametros.procesamientoGasNatural != null) {
        parametros.procesamientoGasNatural.volumen = $("#txtProcesamientoGNVolumen").val();
    }
    $('.data-tbl-Procesamiento-gasnatural-seco').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.procesamientoGasNaturalSeco.length; i++) {
            if (parametros.procesamientoGasNaturalSeco[i].item == datoId) {
                parametros.procesamientoGasNaturalSeco[i].volumen = $("#txtProcGasNaturalSeco_" + datoId).val().length > 0 ? $("#txtProcGasNaturalSeco_" + datoId).val() : null;
            }
        }
    });

    if (parametros.produccionLgn != null) {
        parametros.produccionLgn.volumen = $("#txtProduccionLgnVolumen").val();
    }
    if (parametros.procesamientoLiquidos != null) {
        parametros.procesamientoLiquidos.volumen = $("#ProcesamientoLiquidos").val();
    }

    $('.data-tbl-productos-obtenidos').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.productosObtenido.length; i++) {            
            if (parametros.productosObtenido[i].item == datoId) {                
                parametros.productosObtenido[i].volumen = $("#txtProdObtenidoVolumen_" + datoId).val().length > 0 ? $("#txtProdObtenidoVolumen_" + datoId).val() : null;
            }
        }
    });

    $('.data-tbl-almacenamiento').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.almacenamiento.length; i++) {
            if (parametros.almacenamiento[i].item == datoId) {
                parametros.almacenamiento[i].volumen = $("#TblAlmacenamientoVol_" + datoId).val().length > 0 ? $("#TblAlmacenamientoVol_" + datoId).val() : null;
            }
        }
    });
    parametros.eventoOperativo = $("#txt_EventoOperativo").val();
    var url = $('#__URL_GUARDAR_REPORTE').val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(jqXHR) {
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = jqXHR.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}