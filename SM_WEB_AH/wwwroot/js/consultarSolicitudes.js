$(function () {

    new DataTable('#tblSolicitudes', {
        responsive: true,
        pageLength: 10,
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.4/i18n/es-ES.json'
        }
    });
});

$(document).on("click", ".btn-cancelar", function () {

    var consecutivo = $(this).data("consecutivo");
    var titulo = $(this).data("titulo");

    Swal.fire({
        text: "¿Desea cancelar la solicitud " + titulo + "?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sí",
        cancelButtonText: "No"
    }).then((result) => {
        if (!result.isConfirmed)
            return;

        $.ajax({
            url: "/Solicitud/CancelarSolicitud",
            method: "POST",
            data: {
                consecutivoSolicitud: consecutivo
            },
            dataType: "json",
            success: function (data) {

                swal.fire({
                    title: "Información",
                    text: data,
                    icon: "info",
                    confirmButtonText: "Aceptar"
                }).then(() => {
                    location.reload();
                });
            }
        })
    });
});