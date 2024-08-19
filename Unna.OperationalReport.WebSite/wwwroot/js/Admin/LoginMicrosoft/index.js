$(document).ready(function () {
    controles();
});

function controles() {
    iniciarAutenticacionMicrosoft();
}

function iniciarAutenticacionMicrosoft() {
    var url = $('#__HD_LOGIN_MICROSOFT').val();
    window.location.href = url; // Redirige al usuario a la URL de autenticación
}
