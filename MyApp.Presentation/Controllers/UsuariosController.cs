using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;
using MyApp.Entities.Models;

namespace MyApp.Presentation.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = await _usuarioService.Autenticar(email, password);
            if (usuario != null)
            {
                // lógica para autenticar
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("Rol", usuario.Rol);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(string nombre, string email, string password)
        {
            var usuario = new Usuario { Nombre = nombre, Email = email };
            await _usuarioService.CrearUsuario(usuario, password);
            return RedirectToAction("Login");
        }
    }

}
