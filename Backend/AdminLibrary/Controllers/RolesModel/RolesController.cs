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
        #region Instancia para acceder a la base de datos
        private readonly AppDbContext _context = context;
        #endregion

        #region Obtener lista de roles
        [HttpGet("GetRoles")]
        public async Task<List<RolesDto>> GetMaterials()
        {
            var listMenus = await _context.Roles.ToListAsync();

            if (listMenus.Count == 0)
            {
                return [];
            }

            return listMenus.Select(m => new RolesDto(
                                        m.Id,
                                        m.Name,
                                        m.Description,
                                        m.Status
                                    )).ToList();
        }
        #endregion

        #region Creación de un nuevo rol
        [HttpPost("Create")]
        public async Task<ResponseDto> CreateRole(
            RoleControllerRequest roleControllerRequest
            )
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(roleControllerRequest.Name) ||
                    string.IsNullOrEmpty(roleControllerRequest.Description))
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var isExist = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleControllerRequest.Name);

                if (isExist != null)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.existsRol);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                var roles = new Roles(roleControllerRequest.Name,
                                      roleControllerRequest.Description,
                                      roleControllerRequest.status);

                await _context.Roles.AddAsync(roles);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.success);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }
                else
                {
                    var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                    response.Code = failed?.Code ?? string.Empty;
                    response.Message = failed?.Message;
                }


            }
            catch (Exception ex)
            {
                var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;    

            }
            return response;
        }

        #endregion

        #region Actual un rol existente
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMaterial(
          RoleControllerRequestUpdate roleControllerRequest
          )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleControllerRequest.Id);

                if (isExist != null)
                {
                    isExist.Name = roleControllerRequest.Name;
                    isExist.Description = roleControllerRequest.Description;
                    isExist.Status = roleControllerRequest.status;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "Admin";

                    _context.Roles.Update(isExist);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.success);
                        response.Code = success?.Code ?? string.Empty;
                        response.Message = success?.Message;
                    }
                    else
                    {
                        var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                        response.Code = failed?.Code ?? string.Empty;
                        response.Message = failed?.Message;
                    }
                }
                else
                {
                    var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.updateFailed);
                    response.Code = failed?.Code ?? string.Empty;
                    response.Message = failed?.Message;
                }
            }
            catch (Exception ex)
            {
                var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;
            }

            return response;
        }
        #endregion

    }
}
