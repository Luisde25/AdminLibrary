using AdminLibrary.Controllers.Materials.request;
using AdminLibrary.Controllers.RolesModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using AdminLibrary.Models.Shared;
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
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.exists);

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
                response.Message = failed?.Message;

            }
            return response;
        }

        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMaterial(
          RoleControllerRequestUpdate roleControllerRequest
          )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

                var isExist = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleControllerRequest.Id);

                if (isExist != null)
                {
                    isExist.Name = roleControllerRequest.Name;
                    isExist.Description = roleControllerRequest.Description;
                    isExist.Status = roleControllerRequest.status;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "User";

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
                response.Message = failed?.Message;
            }

            return response;
        }


    }
}
