using CALE.Data;
using CALE.Models;
using CALE.Utils;
using CALE.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CALE.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDataContext _context;

        public AuthController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel login)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null || !PasswordUtils.VerifyPassword(login.Contrasenia, user.Contrasenia))
            {
                TempData["Error"] = "Correo o contraseña incorrectos";
                return RedirectToAction("Login");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Mascota");
        }


        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == registerViewModel.Email);

            if(existingUser != null)
            {
                TempData["Error"] = "El correo ya está registrado";
                return RedirectToAction("Register");
            }

            var hashedPassword = PasswordUtils.HashPassword(registerViewModel.Contrasenia);

            var newUser = new Usuario
            {
                Nombre = registerViewModel.Nombre,
                Telefono = registerViewModel.Telefono,
                Email = registerViewModel.Email,
                Contrasenia = hashedPassword
            };

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, newUser.Email),
                new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }
    }
}
