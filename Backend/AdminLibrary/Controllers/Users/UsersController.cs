using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers.Users
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
