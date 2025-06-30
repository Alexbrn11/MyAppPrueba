using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Entities.Models;
using MyApp.Presentation.Models;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    // Registro GET
    [HttpGet]
    public IActionResult Register() => View();

    // Registro POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nombre = model.Nombre };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Asignar rol Operador por defecto
            if (!await _roleManager.RoleExistsAsync("Operador"))
                await _roleManager.CreateAsync(new IdentityRole("Operador"));

            await _userManager.AddToRoleAsync(user, "Operador");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    // Login GET
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Login POST
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (result.Succeeded)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");
            else
                return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("", "Email o contraseña incorrectos");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    // Gestión de roles (solo admin)
    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public IActionResult Roles()
    {
        // Obtener lista de usuarios y sus roles
        var users = _userManager.Users.ToList();
        var model = users.Select(u => new UserRoleViewModel
        {
            UserId = u.Id,
            Email = u.Email,
            Nombre = u.Nombre,
            RolActual = _userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? "Sin rol"
        }).ToList();

        return View(model);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> CambiarRol(string userId, string nuevoRol)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);

        if (!await _roleManager.RoleExistsAsync(nuevoRol))
            await _roleManager.CreateAsync(new IdentityRole(nuevoRol));

        await _userManager.AddToRoleAsync(user, nuevoRol);

        return RedirectToAction("Roles");
    }

    // Acceso denegado
    [HttpGet]
    public IActionResult AccessDenied() => View();
}
