using Microsoft.AspNetCore.Mvc;

namespace CALE.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}