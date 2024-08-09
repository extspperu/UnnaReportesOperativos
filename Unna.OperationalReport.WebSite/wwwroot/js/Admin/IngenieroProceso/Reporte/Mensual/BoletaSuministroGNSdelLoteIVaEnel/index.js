
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
    
    Guardar(data);
}

function ObtenerError(data) {
    console.log(data);
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function Guardar(parametros) {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();

    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.boletaSuministroGNSdelLoteIVaEnelDet.length; i++) {
            if (parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].id == datoId) {
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].volumen = $("#volumneMPC_" + datoId).val().length > 0 ? $("#volumneMPC_" + datoId).val() : null;
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].poderCalorifico = $("#pCBTUPC_" + datoId).val().length > 0 ? $("#pCBTUPC_" + datoId).val() : null;
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].energia = $("#energiaMMBTU_" + datoId).val().length > 0 ? $("#energiaMMBTU_" + datoId).val() : null;                
            }

        }
    });
    parametros.totalVolumen = $("#totalVolumenMPC").val();
    parametros.totalPoderCalorifico = $("#totalPCBTUPC").val();
    parametros.totalEnergia = $("#totalEnergiaMMBTU").val();
    parametros.totalEnergiaTransferido = $("#totalEnergiaVolTransferidoMMBTU").val();
    parametros.comentarios = $("#txtComentarios").val();   
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