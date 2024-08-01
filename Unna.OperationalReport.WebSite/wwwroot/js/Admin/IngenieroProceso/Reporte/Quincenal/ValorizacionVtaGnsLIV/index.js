
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
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {
    console.log("dato: ", data);
    Guardar(data);
}

function ObtenerError(data) {
    console.log(data);
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
}

function Guardar(parametros) {
    var url = $('#__URL_GUARDAR_REPORTE').val();
    $('.list-datos-tabla').each(function (index) {
        var datoId = $(this).attr('data-id-dato');
        for (var i = 0; i < parametros.valorizacionVtaGnsDet.length; i++) {
            if (parametros.valorizacionVtaGnsDet[i].item == datoId || parametros.valorizacionVtaGnsDet[i].item == null) {
                parametros.valorizacionVtaGnsDet[i].volumen = $("#Volumen_" + datoId).val().length > 0 ? $("#Volumen_" + datoId).val() : null;
                parametros.valorizacionVtaGnsDet[i].poderCal = $("#PoderCal_" + datoId).val().length > 0 ? $("#PoderCal_" + datoId).val() : null;
                parametros.valorizacionVtaGnsDet[i].energia = $("#Energia_" + datoId).val().length > 0 ? $("#Energia_" + datoId).val() : null;
                parametros.valorizacionVtaGnsDet[i].precio = $("#Precio_" + datoId).val().length > 0 ? $("#Precio_" + datoId).val() : null;
                parametros.valorizacionVtaGnsDet[i].costo = $("#Costo_" + datoId).val().length > 0 ? $("#Costo_" + datoId).val() : null;
            }
        }
    });

    parametros.totalVolumen = $("#tbTotalVolumen").val();
    parametros.totalPoderCal = $("#tbTotalPoderCal").val();
    parametros.totalEnergia = $("#tbTotalEnergia").val();
    parametros.totalPrecio = $("#tbTotalPrecio").val();
    parametros.totalCosto = $("#tbTotalCosto").val();

    parametros.enerVolTransM = $("#EnerVolTransM").val();
    parametros.subTotalFact = $("#SubTotalFact").val();
    parametros.igv = $("#Igv").val();
    parametros.totalFact = $("#TotalFact").val();
    parametros.comentario = $("#comentario").val();
    realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
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
    var mediciones = [];
    console.log("Iniciando la captura de datos");
    console.log("URL del endpoint:", url);

    $('#tblValorizacionVtaGns tbody tr').each(function (index) {
        var fecha = $(this).find('td:eq(0)').text().trim();
        console.log("Procesando fila:", index, "Fecha:", fecha);
        if (fecha.toLowerCase() !== "total") {
            var dia = fecha.split('/')[0];
            console.log("Día extraído:", dia);

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

            console.log(`Valores extraídos: Volumen: ${volumen}, PoderCal: ${poderCalorifico}, Energia: ${energia}, Precio: ${precio}, Costo: ${costo}`);

            mediciones.push({ id: volumenId, valor: volumen });
            mediciones.push({ id: poderCalId, valor: poderCalorifico });
            mediciones.push({ id: energiaId, valor: energia });
            mediciones.push({ id: precioId, valor: precio });
            mediciones.push({ id: costoId, valor: costo });
        }
    });

    // Añadir los nuevos inputs como doubles
    var enerVolTransM = parseFloat($('#EnerVolTransM').val().trim()) || 0.0;
    var subTotalFact = parseFloat($('#SubTotalFact').val().trim()) || 0.0;
    var igv = parseFloat($('#Igv').val().trim()) || 0.0;
    var totalFact = parseFloat($('#TotalFact').val().trim()) || 0.0;
    console.log(enerVolTransM);


    const { mesActual, anioActual } = obtenerMesYAnioActual();
    var comentario = $('#comentario').val().trim();

    var valorizacionVtaGnsDto = {
        IdUsuario: 0,
        Mes: mesActual,
        Anio: anioActual.toString(),
        Comentario: comentario,
        EnerVolTransM: enerVolTransM,
        SubTotalFact: subTotalFact,
        Igv: igv,
        TotalFact: totalFact,
        Mediciones: mediciones
    };

    console.log("Datos recopilados para enviar:");
    console.log(valorizacionVtaGnsDto);

    // Usando realizarPost para manejar el envío de datos
    realizarPost(url, valorizacionVtaGnsDto, 'json', RespuestaGuardar, GuardarError, 10000);
}





function RespuestaGuardar(data) {
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("Se guardó correctamente", "success");

}

function GuardarError(data) {
    $("#btnGuardar").html('<i class="far fa-save"></i> Guardar');
    $("#btnGuardar").prop("disabled", false);
    MensajeAlerta("No se pudo completar el registro", "error");
}