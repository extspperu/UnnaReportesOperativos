var ListaDocumentos = [];

$(document).ready(function () {
    controles();
});
function controles() {

    $('#btnGuardarAdjuntos').click(function () {
        GuardarAdjuntos();
    });
    Obtener();
}

function cargarDatos(id) {
    $('#__HD_ID').val(id);
    Obtener();
}

function AdjuntarDocumento(cadena) {
    var ListaDocumentos = [];
    $("#archivosAdjuntos").html("");
    $("#agregarDocumentosModal").modal("show");
    if (cadena == null) {
        return;
    }
    ListaDocumentos = JSON.parse(cadena);

    var html = "";
    for (var i = 0; i < ListaDocumentos.length; i++) {
        html += '<p class="d-flex justify-content-between"><a href="' + ListaDocumentos[i].url + '" target="_blank" >' + ListaDocumentos[i].nombre + '</a></p>';
    }
    $("#archivosAdjuntos").html(html);


}

function AdjuntarComentario(comentario) {
    $("#txtComentario").val("");
    $("#agregarComentarioModal").modal("show");
    $("#txtComentario").val(comentario);
}






function Obtener() {
    var url = $('#__URL_OBTENER').val() + $('#__HD_ID').val();
    var dato = {
    };
    realizarGet(url, dato, 'json', RespuestaObtener, ObtenerError, 10000);
}

function RespuestaObtener(data) {

    if (data.registros.length > 0) {
        for (var i = 0; i < data.registros.length; i++) {
            $('#checkConciliado_' + $('#__HD_ID').val() + '-' + data.registros[i].idDato).prop('checked', data.registros[i].esConciliado);
            $('#txtValorDato_' + $('#__HD_ID').val() + '-' + data.registros[i].idDato).val(data.registros[i].valor);
        }
    }
}

function ObtenerError(data) {
    console.log(data);
}