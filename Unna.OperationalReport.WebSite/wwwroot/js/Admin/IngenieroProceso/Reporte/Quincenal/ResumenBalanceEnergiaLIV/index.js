
var parametros;
$(document).ready(function () {
    controles();

});


function controles() {
    $('#btnDescargarExcel').click(function () {
        console.log("Iniciando EXCEL");

        descargarExcel();
    });
    $('#btnDescargarPdf').click(function () {
        console.log("Iniciando PDF ");

        descargarPdf();
    });
    $('#btnGuardar').click(function () {
        Obtener();
    });

    $('#btnDescargarLGNExcel').click(function () {
        console.log("Iniciando excel LGN");
        descargarLGNExcel();
    });
    $('#btnDescargarLGNPdf').click(function () {
        console.log("Iniciando PDF LGN");
        descargarLGNPdf();
    });

}

function descargarExcel() {
    window.location = $("#__URL_GENERAR_REPORTE_EXCEL").val();
}
function descargarPdf() {
    window.location = $("#__URL_GENERAR_REPORTE_PDF").val();
}

function descargarLGNExcel() {
    var url = $("#__URL_GENERAR_REPORTELGN_EXCEL").val();
    console.log(url); 
    window.location = url;
}
function descargarLGNPdf() {
    window.location = $("#__URL_GENERAR_REPORTELGN_PDF").val();
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
    Guardar();
}

function ObtenerError(data) {
    console.log(data);
}
function obtenerMesYAnioActual() {
    const meses = [
        "enero", "febrero", "marzo", "abril", "mayo", "junio",
        "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
    ];

    const fechaActual = new Date();
    const mesActual = meses[fechaActual.getMonth()];
    const anioActual = fechaActual.getFullYear();

    return { mesActual, anioActual };
}
function guardarDatos() {
    var url = $('#__URL_GUARDAR_REPORTE').val();
    var datosDiarios = [];
    console.log("Iniciando la captura de datos");
    console.log("URL del endpoint:", url);

    $('.tbl-resumen-ventas tbody tr').each(function () {
        var dia = $(this).find('td:eq(0)').text().trim();
        var fila = { Dia: dia, Mediciones: [] };

        $(this).find('input[type="text"]').each(function () {
            var id = $(this).attr('id');
            var valor = $(this).val();
            fila.Mediciones.push({
                ID: id,
                Valor: parseFloat(valor) || valor
            });
        });

        datosDiarios.push(fila);
    });

    // Capturar los valores de los inputs especificados
    var parametrosLGN = {};
    var inputsEspecificos = [
        "DensidadGLPKgBl", "PCGLPMMBtuBl60F", "PCCGNMMBtuBl60F", "PCLGNMMBtuBl60F", "FactorConversionSCFDGal",
        "EnergiaMMBTUQ1GLP", "EnergiaMMBTUQ1CGN", "DensidadGLPKgBlQ2", "PCGLPMMBtuBl60FQ2",
        "PCCGNMMBtuBl60FQ2", "PCLGNMMBtuBl60FQ2", "FactorConversionSCFDGalQ2", "EnergiaMMBTUQ2GLP", "EnergiaMMBTUQ2CGN"
    ];

    inputsEspecificos.forEach(function (id) {
        var input = $('#' + id);
        if (input.length) {
            var valor = parseFloat(input.val()) || 0;
            parametrosLGN[id] = valor;
        }
    });

    const { mesActual, anioActual } = obtenerMesYAnioActual();

    var BoletaVolumenesUNNAEnergiaCNPCDto = {
        IdUsuario: 0,
        Mes: mesActual,
        Anio: anioActual.toString(),
        DatosDiarios: datosDiarios,
        ParametrosLGN: parametrosLGN 
    };

    console.log("Datos recopilados para enviar:");
    console.log(BoletaVolumenesUNNAEnergiaCNPCDto);

    // Usando realizarPost para manejar el envío de datos
    realizarPost(url, BoletaVolumenesUNNAEnergiaCNPCDto, 'json', RespuestaGuardar, GuardarError, 10000);
}

function RespuestaGuardar(response) {
    console.log('Guardado con éxito:', response);
    MensajeAlerta("Datos guardados correctamente", "success");
}

function GuardarError(jqXHR, textStatus, errorThrown) {
    console.error('Error al intentar guardar los datos:', textStatus, errorThrown);
    MensajeAlerta("Error al guardar los datos", "error");
}



function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}