// js/site.js
$(document).ready(function () {
    $("#loginForm").on("submit", async function (e) {
        e.preventDefault();

        const email = $("#email").val();
        const password = $("#password").val();

        try {
            const res = await fetch("/api/auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });

            if (res.ok) {
                const user = await res.json();
                localStorage.setItem("usuario", JSON.stringify(user));
                window.location.href = "/articulos/index.html";  // redirige al módulo de artículos
            } else {
                $("#loginError").text("Credenciales incorrectas.");
            }
        } catch (error) {
            console.error("Error:", error);
            $("#loginError").text("Ocurrió un error inesperado.");
        }
    });
});
