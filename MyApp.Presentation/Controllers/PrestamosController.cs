using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApp.DataAccess;
using MyApp.Entities.Models;
using System.Security.Claims;


namespace MyApp.Presentation.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly AppDbContext _context;

        public PrestamosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Prestamos/Solicitar
        public IActionResult Solicitar()
        {
            ViewBag.ArticulosDisponibles = _context.Articulos
                .Where(a => a.Estado == "Disponible")
                .ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Solicitar(int articuloId, DateTime fechaEntrega)
        {
            var usuarioId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var prestamo = new Prestamo
            {
                ArticuloId = articuloId,
                UsuarioId = usuarioId,
                FechaEntrega = fechaEntrega,
                Estado = "Pendiente"
            };

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Historial");
        }

        // GET: /Prestamos/Aprobar/5
        public async Task<IActionResult> Aprobar(int id)
        {
            var prestamo = await _context.Prestamos.Include(p => p.Articulo).FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null) return NotFound();

            prestamo.Estado = "Aprobado";
            prestamo.Articulo.Estado = "Prestado";
            await _context.SaveChangesAsync();

            return RedirectToAction("Historial");
        }

        // GET: /Prestamos/Rechazar/5
        public async Task<IActionResult> Rechazar(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null) return NotFound();

            prestamo.Estado = "Rechazado";
            await _context.SaveChangesAsync();
            return RedirectToAction("Historial");
        }

        // GET: /Prestamos/Devolver/5
        public async Task<IActionResult> Devolver(int id)
        {
            var prestamo = await _context.Prestamos.Include(p => p.Articulo).FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null) return NotFound();

            prestamo.Estado = "Devuelto";
            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.Articulo.Estado = "Disponible";

            await _context.SaveChangesAsync();
            return RedirectToAction("Historial");
        }

        // GET: /Prestamos/Historial
        public async Task<IActionResult> Historial(string estado, string? usuarioId, int? articuloId)
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Articulo)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                prestamos = prestamos.Where(p => p.Estado == estado);

            if (!string.IsNullOrEmpty(usuarioId))
                prestamos = prestamos.Where(p => p.UsuarioId == usuarioId);

            if (articuloId.HasValue)
                prestamos = prestamos.Where(p => p.ArticuloId == articuloId.Value);

            return View(await prestamos.ToListAsync());
        }
    }
}
