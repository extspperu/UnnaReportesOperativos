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
    //Obtener();
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
}

function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();
    console.log(url);
    console.log(parametros);
    $('.list-datos-tabla').each(function (index) {
        var datoIdSinSlashes = $(this).attr('data-id-dato');
        var datoId = datoIdSinSlashes.replace(/\//g, '');
        for (var i = 0; i < parametros.boletaSuministroGNSdelLoteIVaEnelDet.length; i++) {
            if (parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].fecha == datoId) {
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].volumneMPC = $("#volumneMPC_" + datoId).val().length > 0 ? $("#volumneMPC_" + datoId).val() : null;
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].pCBTUPC = $("#pCBTUPC_" + datoId).val().length > 0 ? $("#pCBTUPC_" + datoId).val() : null;
                parametros.boletaSuministroGNSdelLoteIVaEnelDet[i].energiaMMBTU = $("#energiaMMBTU_" + datoId).val().length > 0 ? $("#energiaMMBTU_" + datoId).val() : null;
                
            }

        }
    });
    parametros.periodo = $("#periodo").val();
    parametros.totalVolumenMPC = $("#totalVolumenMPC").val();
    parametros.totalPCBTUPC = $("#totalPCBTUPC").val();
    parametros.totalEnergiaMMBTU = $("#totalEnergiaMMBTU").val();
    parametros.totalEnergiaVolTransferidoMMBTU = $("#totalEnergiaVolTransferidoMMBTU").val();
   
    console.log('Envio Post');
    console.log(parametros);
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
   
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");
    $("#btnGuardar").html('Guardar');
    $("#btnGuardar").prop("disabled", false);
}