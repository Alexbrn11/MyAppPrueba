using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccess.Repositories;
using MyApp.Entities.Models;

public class ArticulosController : Controller
{
    private readonly IGenericRepository<Articulo> _articuloRepo;

    public ArticulosController(IGenericRepository<Articulo> articuloRepo)
    {
        _articuloRepo = articuloRepo;
    }

    // GET: /Articulos
    public async Task<IActionResult> Index(string buscar, string categoria, string estado)
    {
        var articulos = await _articuloRepo.GetAll();

        if (!string.IsNullOrEmpty(buscar))
            articulos = articulos.Where(a => a.Nombre.Contains(buscar) || a.Codigo.Contains(buscar));

        if (!string.IsNullOrEmpty(categoria))
            articulos = articulos.Where(a => a.Categoria == categoria);

        if (!string.IsNullOrEmpty(estado))
            articulos = articulos.Where(a => a.Estado == estado);

        return View(articulos);
    }

    // GET: /Articulos/Create
    public IActionResult Create() => View();

    // POST: /Articulos/Create
    [HttpPost]
    public async Task<IActionResult> Create(Articulo articulo)
    {
        if (!ModelState.IsValid)
            return View(articulo);

        var existente = (await _articuloRepo.GetAll()).Any(a => a.Codigo == articulo.Codigo);
        if (existente)
        {
            ModelState.AddModelError("Codigo", "El código ya existe.");
            return View(articulo);
        }

        await _articuloRepo.Add(articulo);
        await _articuloRepo.Save();
        return RedirectToAction("Index");
    }

    // GET: /Articulos/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var articulo = await _articuloRepo.GetById(id);
        if (articulo == null) return NotFound();
        return View(articulo);
    }

    // POST: /Articulos/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Articulo articulo)
    {
        if (id != articulo.Id) return BadRequest();

        var existente = (await _articuloRepo.GetAll()).Any(a => a.Codigo == articulo.Codigo && a.Id != id);
        if (existente)
        {
            ModelState.AddModelError("Codigo", "El código ya está en uso por otro artículo.");
            return View(articulo);
        }

        _articuloRepo.Update(articulo);
        await _articuloRepo.Save();
        return RedirectToAction("Index");
    }

    // GET: /Articulos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var articulo = await _articuloRepo.GetById(id);
        if (articulo == null) return NotFound();

        return View(articulo);
    }

    // POST: /Articulos/DeleteConfirmed/5
    [HttpPost, ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var articulo = await _articuloRepo.GetById(id);
        if (articulo == null) return NotFound();

        if (articulo.Estado == "Prestado")
        {
            TempData["Error"] = "No se puede eliminar un artículo con préstamo activo.";
            return RedirectToAction("Index");
        }

        _articuloRepo.Delete(articulo);
        await _articuloRepo.Save();
        return RedirectToAction("Index");
    }
}
