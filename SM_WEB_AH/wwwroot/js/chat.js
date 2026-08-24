let connection = null;
let salaActual = null;

// Clear the sidebar notification badge when the chat page is open
sessionStorage.removeItem('contactoBadge');
(function () { const b = document.getElementById('badgeContacto'); if (b) b.style.display = 'none'; })();

// ─── Conexión SignalR ──────────────────────────────────────────────────────────
function crearConexion() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${urlHub}?access_token=${jwtToken}`)
        .withAutomaticReconnect()
        .build();

    connection.on('RecibirMensaje', (msg) => agregarMensaje(msg));

    connection.onreconnecting(() => {
        document.getElementById('chatEstadoConexion').textContent = 'Reconectando…';
        document.getElementById('chatEstadoConexion').className = 'text-warning';
    });

    connection.onreconnected(() => {
        document.getElementById('chatEstadoConexion').textContent = 'Conectado';
        document.getElementById('chatEstadoConexion').className = 'text-success';
        if (salaActual) connection.invoke('UnirseASala', salaActual);
    });

    return connection.start();
}

// ─── Selección de sala ────────────────────────────────────────────────────────
document.querySelectorAll('.solicitud-item').forEach(el => {
    el.addEventListener('click', () => abrirSala(
        parseInt(el.dataset.consecutivo),
        el.dataset.interlocutor
    ));
});

async function abrirSala(consecutivoSolicitud, nombreInterlocutor) {
    // Marcar elemento activo
    document.querySelectorAll('.solicitud-item').forEach(e => e.classList.remove('active'));
    document.querySelector(`.solicitud-item[data-consecutivo="${consecutivoSolicitud}"]`)
        .classList.add('active');

    salaActual = consecutivoSolicitud;

    // Habilitar caja de envío y actualizar placeholder
    const header = document.getElementById('chatHeader');
    header.style.removeProperty('display');
    document.getElementById('inputMensaje').disabled = false;
    document.getElementById('inputMensaje').placeholder = 'Escribe un mensaje…';
    document.getElementById('btnEnviar').disabled = false;
    document.getElementById('btnEmoji').disabled = false;
    document.getElementById('chatNombreInterlocutor').textContent = nombreInterlocutor;
    document.getElementById('chatEstadoConexion').textContent = 'Conectando…';
    document.getElementById('chatEstadoConexion').className = 'text-warning';

    // Limpiar mensajes anteriores
    const area = document.getElementById('areaMensajes');
    area.innerHTML = '';

    // Iniciar/reutilizar conexión
    if (!connection || connection.state === signalR.HubConnectionState.Disconnected) {
        await crearConexion();
    }

    // Unirse a la sala SignalR
    await connection.invoke('UnirseASala', consecutivoSolicitud);
    document.getElementById('chatEstadoConexion').textContent = 'Conectado';
    document.getElementById('chatEstadoConexion').className = 'text-success';

    // Cargar historial
    await cargarHistorial(consecutivoSolicitud);
}

// ─── Historial vía REST ───────────────────────────────────────────────────────

async function cargarHistorial(consecutivoSolicitud) {
    const res = await fetch(`/Contacto/ConsultarMensajes?consecutivoSolicitud=${consecutivoSolicitud}`);

    if (!res.ok) return;

    const mensajes = await res.json();
    mensajes.forEach(m => agregarMensaje(m, false));
    scrollAbajo();
}

// ─── Emoji picker ────────────────────────────────────────────────────────────

const picker = document.getElementById('emojiPicker');

document.getElementById('btnEmoji').addEventListener('click', () => {
    picker.classList.toggle('d-none');
});

picker.addEventListener('emoji-click', (e) => {
    const emoji = e.detail.unicode;
    const input = document.getElementById('inputMensaje');
    const pos = input.selectionStart ?? input.value.length;
    input.value = input.value.slice(0, pos) + emoji + input.value.slice(pos);
    input.setSelectionRange(pos + emoji.length, pos + emoji.length);
    input.focus();
    picker.classList.add('d-none');
});

document.addEventListener('click', (e) => {
    if (!picker.contains(e.target) && e.target.id !== 'btnEmoji')
        picker.classList.add('d-none');
});


// ─── Envío de mensajes ────────────────────────────────────────────────────────

document.getElementById('btnEnviar').addEventListener('click', enviarMensaje);

document.getElementById('inputMensaje').addEventListener('keydown', (e) => {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        enviarMensaje();
    }
});

async function enviarMensaje() {
    const input = document.getElementById('inputMensaje');
    const texto = input.value.trim();

    if (!texto || !salaActual || connection?.state !== signalR.HubConnectionState.Connected)
        return;

    input.value = '';

    try {
        await connection.invoke('EnviarMensaje', salaActual, texto);
    } catch (e) {
        input.value = texto;
        // SignalR wraps the HubException message — extract only the custom part
        const match = e.message?.match(/HubException:\s*(.+)/);
        const texto_error = match ? match[1] : 'No se pudo enviar el mensaje.';
        Swal.fire({ icon: 'warning', title: 'Mensaje bloqueado', text: texto_error, confirmButtonText: 'Entendido' });
    }
}

// ─── Renderizado de mensajes ──────────────────────────────────────────────────

function agregarMensaje(msg, animar = true) {
    const propio = msg.consecutivoUsuario === consecutivoActual;
    const area = document.getElementById('areaMensajes');

    const wrapper = document.createElement('div');
    wrapper.className = `d-flex mb-2 ${propio ? 'justify-content-end' : 'justify-content-start'}`;

    const hora = new Date(msg.fechaHora).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    wrapper.innerHTML = `
    <div class="chat-burbuja ${propio ? 'burbuja-propia' : 'burbuja-ajena'}">
      ${!propio ? `<div class="chat-nombre">${escapeHtml(msg.nombreUsuario)}</div>` : ''}
      <div class="chat-texto">${escapeHtml(msg.mensaje)}</div>
      <div class="chat-hora">${hora}</div>
    </div>`;

    if (animar) wrapper.classList.add('chat-nuevo');
    area.appendChild(wrapper);
    scrollAbajo();
}

function scrollAbajo() {
    const area = document.getElementById('areaMensajes');
    area.scrollTop = area.scrollHeight;
}

function escapeHtml(texto) {
    return texto
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}