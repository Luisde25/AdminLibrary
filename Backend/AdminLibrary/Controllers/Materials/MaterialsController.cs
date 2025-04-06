using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.audioVisual
{

    [ApiController]
    [Route("api/Materials")]
    public class MaterialsController(
            AppDbContext context
            ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public List<Materials> GetMaterials()
        {
            var listMaterials = _context.Materials.ToList();    

            
            return listMaterials;
        }
    }
}
