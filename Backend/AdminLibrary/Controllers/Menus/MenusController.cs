using AdminLibrary.Dtos;
using AdminLibrary.Models;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.Menus
{
    [ApiController]
    [Route("api/Menus")]
    public class MenusController(
           AppDbContext context
        ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet("GetMenus")]
        public async Task<List<MenusDto>> GetMaterials()
        {
           
            var listMenus = await _context.Menus.ToListAsync();

            if (listMenus.Count == 0)
            {
                return [];
            }

            return listMenus.Select(m => new MenusDto(
                                        m.Name,
                                        m.Url,
                                        m.Father,
                                        m.Order,
                                        m.Status
                                    )).ToList();
            
        }
    }
}
