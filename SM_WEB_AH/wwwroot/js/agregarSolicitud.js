$(function () {

    $.validator.addMethod("formato", function (value, element) {
        if (element.files && element.files[0]) {
            return element.files[0].type === "application/pdf";
        }
        return true;
    }, "Solo se permiten archivos PDF");

    $.validator.addMethod("tamanno", function (value, element) {
        if (element.files && element.files[0]) {
            return element.files[0].size <= 10 * 1024 * 1024;
        }
        return true;
    }, "El archivo no debe superar los 10 MB");

    $("#AgregarSolicitudForm").validate({
        ignore: ":hidden:not([name='Imagen'])",
        rules: {
            Titulo: {
                required: true,
                minlength: 25
            },
            Descripcion: {
                required: true,
                minlength: 100
            },
            Imagen: {
                required: true,
                formato: true,
                tamanno: true
            }
        },
        messages: {
            Titulo: {
                required: "Campo obligatorio",
                minlength: "Mínimo 25 caracteres"
            },
            Descripcion: {
                required: "Campo obligatorio",
                minlength: "Mínimo 100 caracteres"
            },
            Imagen: {
                required: "Campo obligatorio",
                formato: "Solo se permiten archivos PDF",
                tamanno: "El archivo no debe superar los 10 MB"
            }
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.addClass("text-danger small d-block");
            if (element.attr("name") === "Imagen") {
                $("#zonaPDF").after(error);
            } else {
                element.closest(".form-group").after(error);
            }
        },
        highlight: function (element) {
            if (element.name === "Imagen") {
                $("#zonaPDF").css("border-color", "#dc3545");
            } else {
                $(element).addClass("is-invalid");
            }
        },
        unhighlight: function (element) {
            if (element.name === "Imagen") {
                $("#zonaPDF").css("border-color", "");
            } else {
                $(element).removeClass("is-invalid").addClass("is-valid");
            }
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});

function previsualizarPDF(input) {
    const preview = document.getElementById('previstaPDF');
    const texto = document.getElementById('textoPDF');
    if (input.files && input.files[0]) {
        const url = URL.createObjectURL(input.files[0]);
        preview.src = url;
        preview.classList.remove('d-none');
        texto.classList.add('d-none');
    }
}