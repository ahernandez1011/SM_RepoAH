$(function () {

    $.validator.addMethod("caracterEspecial", function (value, element) {
        return this.optional(element) || /[!@#$%^&*(),.?":{}|<>]/.test(value);
    }, "");

    $("#SeguridadForm").validate({
        rules: {
            Contrasenna: {
                required: true,
                minlength: 5,
                caracterEspecial: true
            },
            ConfirmarContrasenna: {
                required: true,
                equalTo: "#Contrasenna"
            },
        },
        messages: {
            Contrasenna: {
                required: "Campo obligatorio",
                minlength: "Mínimo 5 caracteres",
                caracterEspecial: "Al menos 1 caracter especial"
            },
            ConfirmarContrasenna: {
                required: "Campo obligatorio",
                equalTo: "Las contraseñas no coinciden"
            },
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("text-danger small d-block");
            element.closest(".form-group").after(error);
        },
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        },
        submitHandler: function (form) {
            form.submit();
        }
    });

    $("#PerfilForm").validate({
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
            }
        },
        messages: {
            Identificacion: {
                required: "Campo obligatorio"
            },
            Nombre: {
                required: "Campo obligatorio"
            },
            CorreoElectronico: {
                required: "Campo obligatorio",
                email: "Formato no válido"
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("text-danger small d-block");
            element.closest(".form-group").append(error);
        },
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid").addClass("is-valid");
        },
        submitHandler: function (form) {
            form.submit();
        }
    });

});