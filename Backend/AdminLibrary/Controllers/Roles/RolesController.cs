using AdminLibrary.Dtos;
using AdminLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.Roles
{
    [ApiController]
    [Route("api/Roles")]
    public class RolesController(
         AppDbContext context
        ) : Controller
    {
        private readonly AppDbContext _context = context;
       
        [HttpGet("GetRoles")]
        public async Task<List<RolesDto>> GetMaterials()
        {
            var listMenus = await _context.Roles.ToListAsync();

            if (listMenus.Count == 0)
            {
                return [];
            }

            return listMenus.Select(m => new RolesDto(
                                        m.Name,
                                        m.Description,
                                        m.Status
                                    )).ToList();
        }
    }
}
