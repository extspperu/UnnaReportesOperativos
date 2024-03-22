var parametros;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarPdf').click(function () {
        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });
}

function descargarPdf() {
    $("#btnDescargarExcel").html('<i class="fa fa-spinner fa-spin"></i> Cargando...');
    $("#btnDescargarExcel").prop("disabled", true);
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
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

    console.log("dato: ", data);
    parametros = data;
    Guardar();
}

function ObtenerError(data) {
    console.log("error: ", data);
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}



function Guardar() {
    //if (parametros == null) {
    //    MensajeAlerta("No se pudo completar el registro", "error");
    //    return;
    //}
    //if (parametros.datos == null) {
    //    MensajeAlerta("No se pudo completar el registro", "error");
    //    return;
    //}
    //if (parametros.datos.length === 0) {
    //    MensajeAlerta("No se pudo completar el registro", "error");
    //    return;
    //}

    console.log("parametros ", parametros);

    $('.list-datos-tblProductoParaReproceso').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.productoParaReproceso.length; i++) {
            if (parametros.productoParaReproceso[i].item == datoId) {
                parametros.productoParaReproceso[i].nivel = $("#tbl1Nivel_" + datoId).val().length > 0 ? $("#tbl1Nivel_" + datoId).val() : null;
                parametros.productoParaReproceso[i].inventario = $("#tbl1Inventario_" + datoId).val().length > 0 ? $("#tbl1Inventario_" + datoId).val() : null;
            }
        }
    });
    $('.list-datos-tblProductoGlp').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.productoGlp.length; i++) {
            if (parametros.productoGlp[i].item == datoId) {
                parametros.productoGlp[i].nivel = $("#tbl2Nivel_" + datoId).val().length > 0 ? $("#tbl2Nivel_" + datoId).val() : null;
                parametros.productoGlp[i].inventario = $("#tbl2Inventario_" + datoId).val().length > 0 ? $("#tbl2Inventario_" + datoId).val() : null;
            }
        }
    });

    $('.list-datos-tblProductoCgn').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.productoCgn.length; i++) {
            if (parametros.productoCgn[i].item == datoId) {
                parametros.productoCgn[i].nivel = $("#tblCgnNivel_" + datoId).val().length > 0 ? $("#tblCgnNivel_" + datoId).val() : null;
                parametros.productoCgn[i].inventario = $("#tblCgnInventario_" + datoId).val().length > 0 ? $("#tblCgnInventario_" + datoId).val() : null;
            }
        }
    });
    $('.list-datos-productoGlpCgn').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.productoGlpCgn.length; i++) {
            if (parametros.productoGlpCgn[i].item == datoId) {
                parametros.productoGlpCgn[i].produccion = $("#tbProduccion_" + datoId).val().length > 0 ? $("#tbProduccion_" + datoId).val() : null;
                parametros.productoGlpCgn[i].despacho = $("#tbDespacho_" + datoId).val().length > 0 ? $("#tbDespacho_" + datoId).val() : null;
                parametros.productoGlpCgn[i].inventario = $("#tbInventario_" + datoId).val().length > 0 ? $("#tbInventario_" + datoId).val() : null;
            }
        }
    });

    parametros.observacion = $('#tbObservacion').val();
    console.log("parametros2 ", parametros);
    var url = $('#__URL_GUARDAR_REPORTE').val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    console.log("guardar: ", data);
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(jqXHR) {
    console.log("error: ", jqXHR);
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
    var mensaje = jqXHR.responseJSON.mensajes[0];
    MensajeAlerta(mensaje, "info");

}