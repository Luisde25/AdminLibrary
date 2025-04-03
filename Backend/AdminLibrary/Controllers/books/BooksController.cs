using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers.books
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
