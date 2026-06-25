$(function () {

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
                required: "Campo obligatorio",
                email: "Formato no válido"
            },
            Contrasenna: {
                required: "Campo obligatorio",
                minlength: "Mínimo 5 caracteres"
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