using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers.magazine
{
    public class MagazineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
