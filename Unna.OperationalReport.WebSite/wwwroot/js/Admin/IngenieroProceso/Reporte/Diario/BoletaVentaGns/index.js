var parametros;
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
    Obtener();
}

function descargarExcel() {
    window.location = $("#__HD_URL_GENERAR_REPORTE").val();
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
}

function ObtenerError(data) {
    console.log(data);
}



function Guardar() {
    var url = $('#__URL_GUARDAR_REPORTE').val();    
    parametros.mpcs = $("#mpcs").val();
    parametros.btuPcs = $("#btuPcs").val();
    parametros.mmbtu = $("#mmbtu").val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}