using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers.Roles
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
