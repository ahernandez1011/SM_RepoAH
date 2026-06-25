$(document).ready(function () {
    $("#RegistroForm").validate({
        rules: {
            Identificacion: {
                required: true
            },
            Nombre: {
                required: true
            },
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
            Identificacion: {
                required: "Campo obligatorio."
            },
            Nombre: {
                required: "Campo obligatorio."
            },
            CorreoElectronico: {
                required: "Campo obligatorio.",
                email: "Formato no válido."
            },
            Contrasenna: {
                required: "Campo obligatorio.",
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