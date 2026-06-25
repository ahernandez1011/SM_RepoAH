$(document).ready(function () {
    $("#IndexForm").validate({
        rules: {
            CorreoElectronico: {
                required: true,
                email: true
            },
            Contrasenna: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            CorreoElectronico: {
                required: "El correo electrónico es obligatorio.",
                email: "Formato no válido."
            },
            Contrasenna: {
                required: "La contraseña es obligatoria.",
                minlength: "Mínimo 5 caracteres."
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("text-danger small");
            error.insertAfter(element.closest(".form-group"));
        },
        highlight: function (element) {
            $(element).addClass("is-invalid").removeClass("is-valid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        }
    });
});