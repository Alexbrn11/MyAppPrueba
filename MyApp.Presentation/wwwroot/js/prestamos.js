let prestamos = [];

$(document).ready(function () {
    cargarPrestamos();
});

async function cargarPrestamos() {
    try {
        const res = await fetch("/api/prestamos");
        prestamos = await res.json();
        mostrarPrestamos(prestamos);
    } catch (err) {
        console.error("Error al obtener préstamos:", err);
        alert("No se pudieron cargar los datos.");
    }
}

function mostrarPrestamos(lista) {
    const tbody = $("#prestamosBody");
    tbody.empty();

    if (lista.length === 0) {
        tbody.append(`<tr><td colspan="4" class="text-center">No hay préstamos registrados</td></tr>`);
        return;
    }

    lista.forEach(p => {
        tbody.append(`
      <tr>
        <td>${p.articuloNombre}</td>
        <td>${p.usuarioCorreo}</td>
        <td>${new Date(p.fechaSolicitud).toLocaleDateString()}</td>
        <td>${p.estado}</td>
      </tr>
    `);
    });
}

function filtrarPrestamos() {
    const estado = $("#filtroEstado").val();
    const filtrados = estado ? prestamos.filter(p => p.estado === estado) : prestamos;
    mostrarPrestamos(filtrados);
}

function cerrarSesion() {
    localStorage.removeItem("usuario");
    window.location.href = "/index.html";
}
