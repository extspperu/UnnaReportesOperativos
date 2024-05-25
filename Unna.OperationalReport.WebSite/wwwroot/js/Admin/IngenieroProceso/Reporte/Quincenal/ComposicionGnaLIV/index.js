
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
    console.log(url);
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
        for (var i = 0; i < parametros.composicionGnaLIVDetComposicion.length; i++) {
            if (parametros.composicionGnaLIVDetComposicion[i].compGnaDia == datoId) {
                parametros.composicionGnaLIVDetComposicion[i].compGnaC6 = $(this)("#tbCompGnaC6_" + datoId).val().length > 0 ? $(this)("#tbCompGnaC6_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaC3 = $("#tbCompGnaC3_" + datoId).val().length > 0 ? $("#tbCompGnaC3_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaIc4 = $("#tbCompGnaIc4_" + datoId).val().length > 0 ? $("#tbCompGnaIc4_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaNc4 = $("#tbCompGnaNc4_" + datoId).val().length > 0 ? $("#tbCompGnaNc4_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaNeoC5 = $("#tbCompGnaNeoC5_" + datoId).val().length > 0 ? $("#tbCompGnaNeoC5_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaIc5 = $("#tbCompGnaIc5_" + datoId).val().length > 0 ? $("#tbCompGnaIc5_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaNc5 = $("#tbCompGnaNc5_" + datoId).val().length > 0 ? $("#tbCompGnaNc5_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaNitrog = $("#tbCompGnaNitrog_" + datoId).val().length > 0 ? $("#tbCompGnaNitrog_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaC1 = $("#tbCompGnaC1_" + datoId).val().length > 0 ? $("#tbCompGnaC1_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaCo2 = $("#tbCompGnaCo2_" + datoId).val().length > 0 ? $("#tbCompGnaCo2_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaC2 = $("#tbCompGnaC2_" + datoId).val().length > 0 ? $("#tbCompGnaC2_" + datoId).val() : null;
                parametros.composicionGnaLIVDetComposicion[i].compGnaObservacion = $("#tbCompGnaObservacion_" + datoId).val().length > 0 ? $("#tbCompGnaObservacion_" + datoId).val() : null;
            }

        }
    });
    parametros.totalPromedioPeruPetroC6 = $("#totalPromedioPeruPetroC6").val();
    parametros.totalPromedioPeruPetroC3 = $("#totalPromedioPeruPetroC3").val();
    parametros.totalPromedioPeruPetroIc4 = $("#totalPromedioPeruPetroIc4").val();
    parametros.totalPromedioPeruPetroNc4 = $("#totalPromedioPeruPetroNc4").val();
    parametros.totalPromedioPeruPetroNeoC5 = $("#totalPromedioPeruPetroNeoC5").val();
    parametros.totalPromedioPeruPetroIc5 = $("#totalPromedioPeruPetroIc5").val();
    parametros.totalPromedioPeruPetroNc5 = $("#totalPromedioPeruPetroNc5").val();
    parametros.totalPromedioPeruPetroNitrog = $("#totalPromedioPeruPetroNitrog").val();
    parametros.totalPromedioPeruPetroC1 = $("#totalPromedioPeruPetroC1").val();
    parametros.totalPromedioPeruPetroCo2 = $("#totalPromedioPeruPetroCo2").val();
    parametros.totalPromedioPeruPetroC2 = $("#totalPromedioPeruPetroC2").val();
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