using Lab4_Sekcja1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab4_Sekcja1.Controllers
{
    public class BrylyController : Controller
    {
        public IActionResult Index(Prostopadloscian p)
        {
            p.Oblicz();
            return View(p);
        }
    }
}
