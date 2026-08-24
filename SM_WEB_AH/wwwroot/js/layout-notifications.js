(function () {
    const meta = document.getElementById('notificacionesMeta');
    const token = meta?.dataset.token;
    const urlHub = meta?.dataset.urlHub;

    if (!token || !urlHub) return;

    function actualizarBadge(n) {
        const badge = document.getElementById('badgeContacto');
        if (!badge) return;
        if (n > 0) { badge.textContent = n > 99 ? '99+' : n; badge.style.display = ''; }
        else { badge.style.display = 'none'; }
    }

    let count = parseInt(sessionStorage.getItem('contactoBadge') || '0');
    actualizarBadge(count);

    const conn = new signalR.HubConnectionBuilder()
        .withUrl(`${urlHub}?access_token=${token}`)
        .withAutomaticReconnect()
        .build();

    conn.on('NuevoMensaje', function () {
        if (window.location.pathname.toLowerCase().includes('/contacto/chat')) return;
        count++;
        sessionStorage.setItem('contactoBadge', count);
        actualizarBadge(count);
    });

    conn.start().catch(function () { });
})();