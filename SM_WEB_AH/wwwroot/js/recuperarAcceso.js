$(function () {

    $("#RecuperarAccesoForm").validate({
        rules: {
            CorreoElectronico: {
                required: true,
                email: true
            }
        },
        messages: {
            CorreoElectronico: {
                required: "Campo obligatorio",
                email: "Formato no válido"
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