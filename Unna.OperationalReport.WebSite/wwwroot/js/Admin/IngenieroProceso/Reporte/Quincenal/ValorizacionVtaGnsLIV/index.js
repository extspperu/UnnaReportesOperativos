
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
    $('#btnGuardarLGN').click(function () {
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
    var url = $('#__URL_GUARDAR_REPORTE').val();


    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.factoresDistribucionGasNaturalSeco.length; i++) {
            if (parametros.factoresDistribucionGasNaturalSeco[i].item == datoId || parametros.factoresDistribucionGasNaturalSeco[i].item == null) {
                parametros.factoresDistribucionGasNaturalSeco[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].concentracionC1 = $("#ConcentracionC1_" + datoId).val().length > 0 ? $("#ConcentracionC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].volumenC1 = $("#VolumenC1_" + datoId).val().length > 0 ? $("#VolumenC1_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].factoresDistribucion = $("#FactoresDistribucion_" + datoId).val().length > 0 ? $("#FactoresDistribucion_" + datoId).val() : null;
                parametros.factoresDistribucionGasNaturalSeco[i].asignacionGns = $("#AsignacionGns_" + datoId).val().length > 0 ? $("#AsignacionGns_" + datoId).val() : null;
            }
        }
    });


    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
}
function guardarDatos() {
    var url = $('#__URL_GUARDAR_REPORTE').val();
    var mediciones = [];
    console.log("Iniciando la captura de datos");
    console.log("URL del endpoint:", url);

    // Procesar la tabla identificada por el id "tblValorizacionVtaGns"
    $('#tblValorizacionVtaGns tbody tr').each(function () {
        var fecha = $(this).find('td:eq(0)').text().trim();
        if (fecha.toLowerCase() !== "total") {
            var dia = fecha.split('/')[0]; // Extracting the day from the date string

            var volumenId = `Volumen_${dia}`;
            var poderCalId = `PoderCal_${dia}`;
            var energiaId = `Energia_${dia}`;
            var precioId = `Precio_${dia}`;
            var costoId = `Costo_${dia}`;

            var volumen = parseFloat($(`#${volumenId}`).val().trim()) || 0;
            var poderCalorifico = parseFloat($(`#${poderCalId}`).val().trim()) || 0;
            var energia = parseFloat($(`#${energiaId}`).val().trim()) || 0;
            var precio = parseFloat($(`#${precioId}`).val().trim()) || 0;
            var costo = parseFloat($(`#${costoId}`).val().trim()) || 0;

            mediciones.push({ id: volumenId, valor: volumen });
            mediciones.push({ id: poderCalId, valor: poderCalorifico });
            mediciones.push({ id: energiaId, valor: energia });
            mediciones.push({ id: precioId, valor: precio });
            mediciones.push({ id: costoId, valor: costo });
        }
    });

    var valorizacionVtaGnsDto = {
        IdUsuario: 0,  // Este ID se ajustará en el servidor
        Mes: "MES TEST",
        Anio: "ANIO TEST",
        Mediciones: mediciones
    };

    console.log("Datos recopilados para enviar:");
    console.log(valorizacionVtaGnsDto);

    // Usando realizarPost para manejar el envío de datos
    realizarPost(url, valorizacionVtaGnsDto, 'json', RespuestaGuardar, GuardarError, 10000);
}


function RespuestaGuardar(data) {
    MensajeAlerta("Se guardó correctamente", "success");
}

function GuardarError(data) {
    MensajeAlerta("No se pudo completar el registro", "error");

}