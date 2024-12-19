using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CALE.Data;
using CALE.Models;
using CALE.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CALE.Controllers
{
    [Authorize]
    public class MascotaController : Controller
    {
        private readonly AppDataContext _context;

        public MascotaController(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDataContext = _context.Animales
                                        .Include(a => a.Dueno)
                                        .Where(a => a.Estado == "DISPONIBLE");
            return View(await appDataContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animales
                .Include(a => a.Dueno)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Especie,Raza,Edad,Sexo,Estado")] Animal animal, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var duenoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(duenoId != null)
                {
                    animal.DuenoId = Guid.Parse(duenoId);
                }
                else
                {
                    TempData["Error"] = "No estás autenticado para realizar esta acción.";
                    return RedirectToAction("Login", "Auth");
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    Console.WriteLine($"File received: {ImageFile.FileName}, size: {ImageFile.Length} bytes");
                    var uploadResult = CloudinaryUtils.UploadImageFromFile(ImageFile);
                    if (uploadResult != null)
                    {
                        animal.ImageUrl = uploadResult?.Url?.ToString();
                    }
                    else
                    {
                        TempData["Error"] = "La imagen no se pudo cargar.";
                        return View(animal);
                    }
                }
                else
                {
                    TempData["Error"] = "No se ha seleccionado ninguna imagen.";
                    return View(animal);
                }

                animal.Id = Guid.NewGuid();
                _context.Add(animal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "La mascota fue dada en adopción exitósamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(animal);
        }
    }
}
