using AdminLibrary.Dtos;
using AdminLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.Users
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController(
         AppDbContext context
        ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet("GetUsers")]
        public async Task<List<UsersDto>> GetMaterials()
        {
            var listMenus = await _context.Users.ToListAsync();

            if (listMenus.Count == 0)
            {
                return [];
            }

            return listMenus.Select(m => new UsersDto(
                                        m.FirtsName,
                                        m.MiddleName,
                                        m.FirtsLastName,
                                        m.SecondLastName,
                                        m.TypeIdentification,
                                        m.NumberIdentification,
                                        m.Status,
                                        m.UserName,
                                        m.UserType
                                    )).ToList();
        }
    }
}
