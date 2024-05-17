

$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnDescargarExcel').click(function () {
        descargarExcel();
    });
    $('#btnDescargarPdf').click(function () {
        window.location = $("#__HD_URL_GENERAR_REPORTE_PDF").val();
    });
    $('#btnGuardar').click(function () {
         Obtener();
    });

}

function descargarExcel() {
    window.location = $("#__HD_URL_GENERAR_REPORTE_EXCEL").val();
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
}

function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.volumenGna.length; i++) {
            if (parametros.volumenGna[i].id == datoId) {
                parametros.volumenGna[i].gasMpcd = $("#tbGasMpcd_" + datoId).val().length > 0 ? $("#tbGasMpcd_" + datoId).val() : null;
                parametros.volumenGna[i].glpBls = $("#tbGlpBls_" + datoId).val().length > 0 ? $("#tbGlpBls_" + datoId).val() : null;
                parametros.volumenGna[i].cgnBls = $("#tbCgnBls_" + datoId).val().length > 0 ? $("#tbCgnBls_" + datoId).val() : null;
                parametros.volumenGna[i].gnsMpc = $("#tbGnsMpc_" + datoId).val().length > 0 ? $("#tbGnsMpc_" + datoId).val() : null;
                parametros.volumenGna[i].gcMpc = $("#tbGcMpc_" + datoId).val().length > 0 ? $("#tbGcMpc_" + datoId).val() : null;
            }
            
        }
    });
    parametros.totalGasMpcd = $("#totalGasMpcd").val();
    parametros.totalGlpBls = $("#totalGlpBls").val();
    parametros.totalCgnBls = $("#totalCgnBls").val();
    parametros.totalGnsMpc = $("#totalGnsMpc").val();
    parametros.totalGcMpc = $("#totalGcMpc").val();
    parametros.gravedadEspacificoGlp = $("#gravedadEspacificoGlp").val();
    parametros.nota = $("#txtNota").val();
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