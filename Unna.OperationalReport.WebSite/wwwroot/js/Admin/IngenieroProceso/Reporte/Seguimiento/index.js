var parametros;
$(document).ready(function () {
    controles();
});

function controles() {
    Obtener();
}

function Obtener() {
    var url = $('#__URL_OBTENER').val();
    console.log("Intentando ingresar al url ", url);
    var dato = {};
    realizarGet(url, dato, 'json', RespuestaObtener, ErrorObtener, 10000);
}

function realizarGet(url, data, dataType, successCallback, errorCallback, timeout) {
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
        dataType: dataType,
        success: successCallback,
        error: errorCallback,
        timeout: timeout
    });
}

function RespuestaObtener(data) {
    console.log(data);
    parametros = data;
    generarCuadros(data);
}

function generarCuadros(data) {
    var container = $("#columns-container");
    container.empty();

    data.forEach(function (column) {
        var headerColor = 'bg-light';
        var allApproved = true;
        var allRejected = true;

        column.boxes.forEach(function (box) {
            if (box.color !== 'bg-success') {
                allApproved = false;
            }
            if (box.color !== 'bg-danger') {
                allRejected = false;
            }
        });

        if (allApproved) {
            headerColor = 'bg-success';
        } else if (allRejected) {
            headerColor = 'bg-danger';
        }

        var columnDiv = `<div class="column">
            <div class="column-header step-box ${headerColor}" style="color: black">${column.header}</div>`;

        column.boxes.forEach(function (box) {
            if (box.esVisible) {
                columnDiv += `<div class="step-box ${box.color}" style="color: ${box.colorTexto}">${box.titulo}</div>`;
            } else {
                columnDiv += `<div class="step-box ${box.color}" style="color: ${box.colorTexto}"></div>`;
            }
        });

        columnDiv += `</div>`;
        container.append(columnDiv);
    });
}




function ErrorObtener(data) {
    console.log(data);
    $("#contenidoErrorMensaje").show();
}
