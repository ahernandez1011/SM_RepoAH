$(function () {

    // Funcionalidad para mostrar/ocultar contraseña
    $(".toggle-password").on("click", function () {
        var target = $(this).data("target");
        var input = $("#" + target);

        if (input.attr("type") === "password") {
            input.attr("type", "text");
            $(this).removeClass("bi-eye-slash").addClass("bi-eye");
        } else {
            input.attr("type", "password");
            $(this).removeClass("bi-eye").addClass("bi-eye-slash");
        }
    });

    $.validator.addMethod("caracterEspecial", function (value, element) {
        return this.optional(element) || /[!@#$%^&*(),.?":{}|<>]/.test(value);
    }, "");

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
                minlength: 5,
                caracterEspecial: true
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
                minlength: "Mínimo 5 caracteres.",
                caracterEspecial: "Debe contener al menos 1 caracter especial."
            }
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
});