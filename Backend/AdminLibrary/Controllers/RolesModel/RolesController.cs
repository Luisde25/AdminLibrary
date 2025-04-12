using AdminLibrary.Constants;
using AdminLibrary.Controllers.RolesModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.RolesModel
{
    [ApiController]
    [Route("api/Roles")]
    public class RolesController(
         AppDbContext context
        ) : Controller
    {
      
        private readonly AppDbContext _context = context;
        private readonly Roles roles = new();

        #region Obtener lista de roles
        [HttpGet("GetRoles")]
        public async Task<List<RolesDto>> GetRoles()
        {
            return await roles.ListRoles(_context);
        }
        #endregion

        #region Creación de un nuevo rol
        [HttpPost("Create")]
        public async Task<ResponseDto> CreateRole(
            RoleControllerRequest roleControllerRequest
            )
        {
            return await roles.CreateNewRol(_context, roleControllerRequest);
        }

        #endregion

        #region Actual un rol existente
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMaterial(
          RoleControllerRequestUpdate roleControllerRequest
          )
        {
            return await roles.UpdateCurrentRol(_context, roleControllerRequest);
        }
        #endregion

    }
}
