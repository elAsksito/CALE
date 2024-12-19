using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CALE.Data;
using CALE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CALE.Controllers
{
    [Authorize]
    public class AdopcionController : Controller
    {
        private readonly AppDataContext _context;

        public AdopcionController(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var appDataContext = _context.Adopciones
                                    .Include(a => a.Animal)
                                    .Where(a => a.UsuarioId == Guid.Parse(usuarioId))
                                    .OrderByDescending(a => a.FechaAdopcion);
            return View(await appDataContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adopcion = await _context.Adopciones
                .Include(a => a.Animal)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adopcion == null)
            {
                return NotFound();
            }

            return View(adopcion);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["Error"] = "Debe iniciar sesión para adoptar.";
                return RedirectToAction("Login", "Auth");
            }

            var adopcion = new Adopcion
            {
                Id = Guid.NewGuid(),
                AnimalId = id,
                UsuarioId = Guid.Parse(userId),
                FechaAdopcion = DateTime.Now
            };

            _context.Add(adopcion);

            var animal = await _context.Animales.FindAsync(id);
            if (animal != null)
            {
                animal.Estado = "ADOPTADO";
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Adopción realizada exitosamente.";
            return RedirectToAction("Index", "Adopcion");
        }
    }
}
