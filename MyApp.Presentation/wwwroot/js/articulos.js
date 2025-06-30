// js/articulos.js
let articulos = [];
let modal;

$(document).ready(function () {
    modal = new bootstrap.Modal(document.getElementById('modalArticulo'));
    cargarArticulos();

    // Guardar artículo
    $("#formArticulo").on("submit", async function (e) {
        e.preventDefault();
        const id = $("#articuloId").val();
        const articulo = {
            codigo: $("#codigo").val(),
            nombre: $("#nombre").val(),
            categoria: $("#categoria").val(),
            estado: $("#estado").val(),
            ubicacion: $("#ubicacion").val()
        };

        try {
            let res;
            if (id) {
                // Editar artículo
                articulo.id = id;
                res = await fetch(`/api/articulos/${id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(articulo)
                });
            } else {
                // Nuevo artículo
                res = await fetch("/api/articulos", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(articulo)
                });
            }

            if (res.ok) {
                modal.hide();
                await cargarArticulos();
            } else {
                alert("Error al guardar el artículo.");
            }
        } catch (err) {
            console.error(err);
            alert("Error de conexión con el servidor.");
        }
    });
});

async function cargarArticulos() {
    try {
        const res = await fetch("/api/articulos");
        articulos = await res.json();
        mostrarArticulos();
    } catch (err) {
        console.error("Error al cargar artículos:", err);
    }
}

function mostrarArticulos() {
    const tbody = $("#articulosBody");
    tbody.empty();
    if (articulos.length === 0) {
        tbody.append(`<tr><td colspan="6" class="text-center">No hay artículos registrados</td></tr>`);
        return;
    }

    articulos.forEach(a => {
        tbody.append(`
      <tr>
        <td>${a.codigo}</td>
        <td>${a.nombre}</td>
        <td>${a.categoria}</td>
        <td>${a.estado}</td>
        <td>${a.ubicacion}</td>
        <td>
          <button class="btn btn-sm btn-warning me-2" onclick="editarArticulo(${a.id})">Editar</button>
          <button class="btn btn-sm btn-danger" onclick="eliminarArticulo(${a.id})">Eliminar</button>
        </td>
      </tr>
    `);
    });
}

function mostrarFormulario() {
    $("#articuloId").val("");
    $("#formArticulo")[0].reset();
    $("#modalLabel").text("Nuevo Artículo");
    modal.show();
}

function editarArticulo(id) {
    const articulo = articulos.find(a => a.id === id);
    if (!articulo) return;

    $("#modalLabel").text("Editar Artículo");
    $("#articuloId").val(articulo.id);
    $("#codigo").val(articulo.codigo);
    $("#nombre").val(articulo.nombre);
    $("#categoria").val(articulo.categoria);
    $("#estado").val(articulo.estado);
    $("#ubicacion").val(articulo.ubicacion);
    modal.show();
}

async function eliminarArticulo(id) {
    if (!confirm("¿Estás seguro de eliminar este artículo?")) return;

    try {
        const res = await fetch(`/api/articulos/${id}`, {
            method: "DELETE"
        });

        if (res.ok) {
            await cargarArticulos();
        } else {
            alert("Error al eliminar el artículo.");
        }
    } catch (err) {
        console.error(err);
        alert("No se pudo conectar al servidor.");
    }
}

function cerrarSesion() {
    localStorage.removeItem("usuario");
    window.location.href = "/index.html";
}
