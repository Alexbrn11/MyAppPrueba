$(document).ready(function () {
    cargarArticulos();

    $("#formPrestamo").on("submit", async function (e) {
        e.preventDefault();
        const articuloId = $("#articuloSelect").val();
        if (!articuloId) return;

        try {
            const res = await fetch("/api/prestamos", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ articuloId: parseInt(articuloId) })
            });

            if (res.ok) {
                $("#solicitudMsg").html(`<span class="text-success">Préstamo solicitado correctamente.</span>`);
                $("#formPrestamo")[0].reset();
            } else {
                const err = await res.text();
                $("#solicitudMsg").html(`<span class="text-danger">${err || 'Error al solicitar préstamo.'}</span>`);
            }
        } catch (err) {
            console.error(err);
            $("#solicitudMsg").html(`<span class="text-danger">Error de conexión con el servidor.</span>`);
        }
    });
});

async function cargarArticulos() {
    try {
        const res = await fetch("/api/articulos/disponibles");
        const articulos = await res.json();
        const select = $("#articuloSelect");

        articulos.forEach(a => {
            select.append(`<option value="${a.id}">${a.nombre} (${a.codigo})</option>`);
        });
    } catch (err) {
        console.error("Error al cargar artículos disponibles:", err);
    }
}

function cerrarSesion() {
    localStorage.removeItem("usuario");
    window.location.href = "/index.html";
}
