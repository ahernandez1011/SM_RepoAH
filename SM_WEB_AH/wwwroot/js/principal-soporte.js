document.querySelectorAll('.btn-atender').forEach(btn => {
    btn.addEventListener('click', () => {
        const consecutivo = btn.dataset.consecutivo;
        const titulo = btn.dataset.titulo;

        Swal.fire({
            title: 'Resolver solicitud',
            html: `<p class="text-muted small mb-2">${titulo}</p>`,
            input: 'textarea',
            inputLabel: 'Solución brindada',
            inputPlaceholder: 'Describe la solución...',
            inputAttributes: { maxlength: 2000, rows: 5 },
            showCancelButton: true,
            confirmButtonText: 'Guardar',
            confirmButtonColor: '#198754',
            cancelButtonText: 'Cancelar',
            preConfirm: (solucion) => {
                if (!solucion.trim())
                    Swal.showValidationMessage('La solución es requerida');
                return solucion;
            }
        }).then(result => {
            if (!result.isConfirmed) return;

            $.post('/Solicitud/AtenderSolicitud',
                { consecutivo, solucion: result.value },
                () => {
                    Swal.fire({ icon: 'success', title: 'Solicitud resuelta', timer: 1500, showConfirmButton: false })
                        .then(() => location.reload());
                }
            ).fail(() => Swal.fire({ icon: 'error', title: 'No se pudo guardar la solución' }));
        });
    });
});