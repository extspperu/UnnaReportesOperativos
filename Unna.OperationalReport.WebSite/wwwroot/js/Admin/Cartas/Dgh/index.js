var parametros;
$(document).ready(function () {
    controles();
});


function controles() {
    $('#btnGuardar').click(function () {
        Guardar();
    });
    $('#btnGuardarArchivos').click(function () {
        GuardarArchivos();
    });

    $('#btnAgregarPeriodoPrecion').click(function () {
        agregarPeriodoPrecio();
    });
    $('#tbNumero').keypress(function () {
        var valor = paragraph.replace("Ruth's", 'my');
        $("#tbSumilla").val();
    });
    Obtener();
}


function Obtener() {
    $("#contenidoErrorMensaje").hide();
    $("#contenidoCarta").hide();
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function RespuestaObtener(data) {
    //Guardar(data);
    console.log(data);
    $("#contenidoCarta").show();
    parametros = data;
    $("#firmaCarta").html('<img src="' + data.urlFirma + '" style="max-width:160px;" />');
    cargarSolicitud(data.solicitud);
    cargarOsinergmin1(data.osinergmin1);
}


function cargarSolicitud(data) {
    //Guardar(data);

    $("#tbSumilla").val(data.sumilla);
    $("#tbNumero").val(data.numero);
    $("#tbAnio").val(data.anio);
    $("#tbFecha").val(data.fecha);
    $("#tbPeriodo").val(data.periodo);
    $("#tbDestinatario").val(data.destinatario);
    $("#tbAsunto").val(data.asunto);
    $("#tbCuerpo").val(data.cuerpo);
    $("#tbPie").val(data.pie);
    
}

function cargarOsinergmin1(data) {
    
    // Asignar Periodo
    $("#periodoSH1").text(data.periodo);
    $("#periodoSH2").text(data.periodo);

    // Asignar valores a la tabla 1: Recepción de Gas Natural Asociado
    $("#recepcionLoteZ2B").val(data.recepcionGasNaturalAsociado.loteZ69);
    $("#recepcionLoteX").val(data.recepcionGasNaturalAsociado.loteX);
    $("#recepcionLoteVI").val(data.recepcionGasNaturalAsociado.loteVI);
    $("#recepcionLoteI").val(data.recepcionGasNaturalAsociado.loteI);
    $("#recepcionLoteV").val(data.recepcionGasNaturalAsociado.loteIV);
    $("#recepcionTotal").val(data.recepcionGasNaturalAsociado.total);

    // Asignar valores a la tabla 2: Usos del Gas
    $("#usoGasNaturalRestituido").val(data.usoGas.gasNaturalRestituido);
    $("#usoConsumoPropio").val(data.usoGas.consumoPropio);
    $("#usoConvertidoEnLgn").val(data.usoGas.convertidoEnLgn);
    $("#usoTotal").val(data.usoGas.total);

    // Asignar valores a la tabla 3: Producción de Líquidos del Gas Natural
    $("#produccionGlp").val(data.produccionLiquidosGasNatural.glp);
    $("#produccionPropanoSaturado").val(data.produccionLiquidosGasNatural.propanoSaturado);
    $("#produccionButanoSaturado").val(data.produccionLiquidosGasNatural.butanoSaturado);
    $("#produccionHexano").val(data.produccionLiquidosGasNatural.hexano);
    $("#produccionCondensados").val(data.produccionLiquidosGasNatural.condensados);
    $("#produccionPromedioLiquidos").val(data.produccionLiquidosGasNatural.promedioLiquidos);
}

function ErrorObtener(data) {
    //Guardar(data);
    console.log(data);
    $("#contenidoErrorMensaje").show();
}

function GuardarArchivos() {
    const archivos = [
        { element: document.getElementById('fileInventarioLoteIV'), producto: 'INVENTARIO', tipo: 'LOTE IV' },
        { element: document.getElementById('fileInventarioPGT'), producto: 'INVENTARIO', tipo: 'PGT' },
        { element: document.getElementById('fileResumenLoteIV'), producto: 'RESUMEN', tipo: 'LOTE IV' },
        { element: document.getElementById('fileResumenPGT'), producto: 'RESUMEN', tipo: 'PGT' }
    ];

    const archivosSeleccionados = archivos.filter(a => a.element.files.length > 0);

    if (archivosSeleccionados.length === 0) {
        MensajeAlerta("No se seleccionaron archivos para cargar", "error");
        return;
    }

    archivosSeleccionados.forEach(archivoSeleccionado => {
        const file = archivoSeleccionado.element.files[0];
        const reader = new FileReader();

        reader.onload = function (event) {
            const base64File = event.target.result.split(',')[1];  // Obtener solo la parte base64
            const parametros = {
                file: base64File,
                producto: archivoSeleccionado.producto,
                tipo: archivoSeleccionado.tipo,
                idUsuario: 1
            };
            console.log(parametros);
            const url = $('#__URL_GUARDAR_ARCHIVOS').val();
            realizarPost(url, parametros, 'json', RespuestaGuardar, GuardarError, 10000);
        };

        reader.readAsDataURL(file);
    });
}




function Guardar() {
    if (parametros == null) {
        MensajeAlerta("No se pudo completar el registro", "error");
        return;
    }
    var url = $('#__URL_GUARDAR_REPORTE').val();
    parametros.solicitud.sumilla = $("#tbSumilla").val();
    parametros.solicitud.numero = $("#tbNumero").val();
    parametros.solicitud.anio = $("#tbAnio").val();
    parametros.solicitud.fecha = $("#tbFecha").val();
    parametros.solicitud.periodo = $("#tbPeriodo").val();
    parametros.solicitud.destinatario = $("#tbDestinatario").val();
    parametros.solicitud.asunto = $("#tbAsunto").val();
    parametros.solicitud.cuerpo = $("#tbCuerpo").val();
    parametros.solicitud.pie = $("#tbPie").val();

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


// TIPO DE CAMBIO
function ObtenerTipoCambio() {
    var url = $('#__URL_OBTENER_REPORTE').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtenerTipoCambio, ErrorObtenerTipoCambio, 10000);
}
function RespuestaObtenerTipoCambio(data) {
    console.log(data);
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr class="list-datos-tabla" data-id-dato="' + data[i].id + '">' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbVolumen_' + data[i].id + '" value="' + data[i].volumen + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbEnergia_' + data[i].id + '" value="' + data[i].energia + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbEnergia_' + data[i].id + '" value="' + data[i].energia + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbEnergia_' + data[i].id + '" value="' + data[i].energia + '"></td>' +
            '</tr>';
    }
    $("#tblTipoCambioCrud tbody").html(html);
}

function ErrorObtenerTipoCambio(data) {
    console.log(data);
}



// Periodo y precio GLP

function eliminarPrecioquitarLista(id) {
    var url = $('#__URL_ELIMINAR_PERIODO_PRECIO').val() + id;
    var dato = {
    };
    realizarDelete(url, dato, 'json', RespuestaEliminarPrecioquitarLista, ErrorEliminarPrecioquitarLista, 10000);
}
function RespuestaEliminarPrecioquitarLista(data) {
    console.log(data);
    Obtener();
    MensajeAlerta("Se eliminó correctamente", "success");
}
function ErrorEliminarPrecioquitarLista(data) {
    MensajeAlerta("No se puede eliminar, intente nuevamente", "error");
}

function pintarPeriodoPrecio(data) {
    $("#tblPeriodoPrecios").html("");
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr class="list-datos-tabla" id="tblFile_' + data[i].id + '">' +
            '<td> <input type="text" class="form-control form-report campo-fecha" id="tbDesde_' + data[i].id + '" value="' + data[i].desdeCadena + '"></td>' +
            '<td> <input type="text" class="form-control form-report campo-fecha" id="tbHasta_' + data[i].id + '" value="' + data[i].hastaCadena + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbPrecioKg_' + data[i].id + '" value="' + data[i].precioKg + '"></td>' +
            '<td> <input type="text" class="form-control form-report only-number text-right" id="tbNroDias_' + data[i].id + '" value="' + data[i].nroDias + '"></td>' +
            '<td> <button type="button" class="btn btn-light-success font-weight-bold btn-sm mr-1" title="Guardar" onclick="GuardarPrecioPeriodo(\'' + data[i].id + '\',true)"><i class="far fa-save"></i></button>' +
            '<button type="button" class="btn btn-light-danger font-weight-bold  btn-sm" title="Eliminar" onclick="eliminarPrecioquitarLista(\'' + data[i].id + '\')"><i class="flaticon2-rubbish-bin"></i></button></td>' +
            '</tr>';
        $("#tblPeriodoPrecios").append(html);
    }
    campoFechas();
}


function agregarPeriodoPrecio() {
    var html = "";
    var id = preciosGl.length + 1;
    html += '<tr class="list-datos-tabla" id="tblFile_' + id + '">' +
        '<td> <input type="text" class="form-control form-report campo-fecha" id="tbDesde_' + id + '"></td>' +
        '<td> <input type="text" class="form-control form-report campo-fecha" id="tbHasta_' + id + '"></td>' +
        '<td> <input type="text" class="form-control form-report only-number text-right" id="tbPrecioKg_' + id + '"></td>' +
        '<td> <input type="text" class="form-control form-report only-number text-right" id="tbNroDias_' + id + '"></td>' +
        '<td> <button type="button" class="btn btn-light-success font-weight-bold btn-sm mr-1" title="Guardar" onclick="GuardarPrecioPeriodo(' + id + ',false)"><i class="far fa-save"></i></button>' +
        '<button type="button" class="btn btn-light-danger font-weight-bold  btn-sm" title="Eliminar" onclick="quitarLista(' + id + ')"><i class="flaticon2-rubbish-bin"></i></button></td>' +
        '</tr>';
    preciosGl.push(html);
    $("#tblPeriodoPrecios").append(html);
    campoFechas();
}

function quitarLista(id) {
    $("#tblFile_" + id).remove();
}

function generarFechaOrdenado(fecha) {
    const [day2, month2, year2] = fecha.split('/');
    const hasta = [year2, month2, day2].join('-');
    return hasta;
}

function GuardarPrecioPeriodo(id, nuevo) {
    var url = $('#__URL_GUARDAR_PERIODO_PRECIO').val();

    if ($("#tbDesde_" + id).val().length === 0) {
        MensajeAlerta("Fecha desde es requerido", "info");
        return;
    } else if ($("#tbHasta_" + id).val().length === 0) {
        MensajeAlerta("Fecha hasta es requerido", "info");
        return;
    } else if ($("#tbPrecioKg_" + id).val().length === 0) {
        MensajeAlerta("Precio es requerido", "info");
        return;
    }

    var dato = {
        Id: nuevo ? id : null,
        Desde: generarFechaOrdenado($("#tbDesde_" + id).val()),
        Hasta: generarFechaOrdenado($("#tbHasta_" + id).val()),
        PrecioKg: $("#tbPrecioKg_" + id).val(),
        NroDias: $("#tbNroDias_" + id).val(),
    };
    console.log(dato);
    realizarPost(url, dato, 'json', RespuestaGuardarPrecioPeriodo, ErrorGuardarPrecioPeriodo, 10000);
}
function RespuestaGuardarPrecioPeriodo(data) {
    console.log(data);
    MensajeAlerta("Se guardó correctamente", "success");
    Obtener();

}
function ErrorGuardarPrecioPeriodo(data) {
    console.log(data);
    MensajeAlerta("No se puede agregar", "error");
}


// Tipo de cambio
function ListarTipoCambio() {
    var url = $('#__URL_LISTAR_TIPOS_CAMBIO_MES').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaListarTipoCambio, ErrorListarTipoCambio, 10000);
}



var tablaDataTiposCambio = null;
function RespuestaListarTipoCambio(data) {
    $("#modalGuardarTipoCambio").modal("show");
    console.log(data);
    if (tablaDataTiposCambio) {
        tablaDataTiposCambio.destroy();
        tablaDataTiposCambio = null;
    }
    var table = $('#tblTipoCambioVisualExcel').DataTable();
    table.destroy();
    var html = "";
    for (var i = 0; i < data.length; i++) {
        html += '<tr>' +
            '<td>' + data[i].fecha + '</td>' +
            '<td>' + data[i].cambio + '</td>'
        '</tr>';
    }
    $("#tblTipoCambioVisualExcel tbody").html(html);
    tablaDataTiposCambio = $('#tblTipoCambioVisualExcel').DataTable({
        "searching": false,
        "bLengthChange": false,
        //"order": [[1, "desc"]],
        "info": true,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        },
        "aoColumnDefs": [
            {
                'bSortable': false,
                'aTargets': [0]
            }
        ],
    });
}


function ErrorListarTipoCambio(data) {
    MensajeAlerta("No se completo la lista de tipos de cambios", "error");
}

function CargarExcelTipoCambio() {
    $("#btnCargarTipoCambioLabel").prop("disabled", true);
    $("#btnCargarTipoCambioLabel").html('<i class="fa fa-spinner fa-spin" aria-hidden="true"></i>Subiendo...');
    var url = $('#__URL_GUARDAR_DOCUMENTO_EXCEL').val();
    var dato = new FormData($("#FormCargarTipoCambio")[0]);
    $.ajax({
        type: "POST",
        url: url,
        data: dato,
        processData: false,
        contentType: false,
        success: function (data) {
            $("#btnCargarTipoCambioLabel").html("Cargar Excel");
            $("#btnCargarTipoCambioLabel").prop("disabled", false);
            document.getElementById("btnCargarTipoCambio").value = null;
            MensajeAlerta("Se proceso correctamente", "success");
            ListarTipoCambio();
            Obtener();
        },
        error: function (jqXHR, status, error) {
            MensajeAlerta("No se puede completar la carga, intente nuevamente", "error");
            $("#btnCargarTipoCambioLabel").html("Cargar Excel");
            $("#btnCargarTipoCambioLabel").prop("disabled", false);
            document.getElementById("btnCargarTipoCambio").value = null;
        },
    });
}