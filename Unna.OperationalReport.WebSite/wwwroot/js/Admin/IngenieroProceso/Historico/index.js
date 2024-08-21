
$(document).ready(function () {
    controles();
    });

function controles() {

        $('#btn3_GNA').click(function () {
            console.log("Iniciando EXCEL");

            descargar3_GNA();
        });
        $('#btn1b_Tks').click(function () {
            console.log("Iniciando EXCEL");

            descargar1b_Tks();
        });
        $('#btn3_Gas').click(function () {
            console.log("Iniciando EXCEL");

            descargar3_Gas();
        });
        $('#btnExistencias').click(function () {
            console.log("Iniciando EXCEL");

            descargarExistencias();
        });
        $('#btn2_Liquidos').click(function () {
            console.log("Iniciando EXCEL");

            descargar2_Liquidos();
        });
        Obtener();
    }

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    console.log("Intentando ingresar al url ", url);
    var dato = { };
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    console.log(data);
}

function ErrorObtener(data) {
        console.log(data);
        $("#contenidoErrorMensaje").show();
}

function descargar3_GNA() {
    // Mostrar el GIF de carga
    $('#btn3_GNA img.loading').show();

    // Realizar la descarga
    var url = $("#__URL_EXCEL_3_GNA").val();

    $.ajax({
        type: 'GET',
        url: url,
        xhrFields: {
            responseType: 'blob' 
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = '3_GNA.xlsx'; 
            document.body.append(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error en la descarga", errorThrown);
        },
        complete: function () {
            $('#btn3_GNA img.loading').hide();
        }
    });
}
function descargar1b_Tks() {
    // Mostrar el GIF de carga
    $('#btn1b_Tks img.loading').show();

    // Realizar la descarga
    var url = $("#__URL_EXCEL_1b_Tks").val();

    $.ajax({
        type: 'GET',
        url: url,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = '1b_Tks.xlsx';
            document.body.append(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error en la descarga", errorThrown);
        },
        complete: function () {
            $('#btn1b_Tks img.loading').hide();
        }
    });
}
function descargar2_Liquidos() {
    // Mostrar el GIF de carga
    $('#btn2_Liquidos img.loading').show();

    // Realizar la descarga
    var url = $("#__URL_EXCEL_2_Liquidos").val();

    $.ajax({
        type: 'GET',
        url: url,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = '2_Liquidos.xlsx';
            document.body.append(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error en la descarga", errorThrown);
        },
        complete: function () {
            $('#btn2_Liquidos img.loading').hide();
        }
    });
}
function descargar3_Gas() {
    // Mostrar el GIF de carga
    $('#btn3_Gas img.loading').show();

    // Realizar la descarga
    var url = $("#__URL_EXCEL_3_Gas").val();

    $.ajax({
        type: 'GET',
        url: url,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = '3_Gas.xlsx';
            document.body.append(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error en la descarga", errorThrown);
        },
        complete: function () {
            $('#btn3_Gas img.loading').hide();
        }
    });
}
function descargarExistencias() {
    // Mostrar el GIF de carga
    $('#btnExistencias img.loading').show();

    // Realizar la descarga
    var url = $("#__URL_EXCEL_Existencias").val();

    $.ajax({
        type: 'GET',
        url: url,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = 'Existencias.xlsx';
            document.body.append(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error en la descarga", errorThrown);
        },
        complete: function () {
            $('#btnExistencias img.loading').hide();
        }
    });
}

function realizarGet(url, dato, tipo, Funcion, FuncionError, timeout) {
    $.ajax({
        type: 'GET',
        url: url,
        data: dato,
        dataType: tipo,
        success: Funcion,
        error: FuncionError,
        timeout: timeout
    });
    }
