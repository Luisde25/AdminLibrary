using AdminLibrary.Controllers.UsersModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using AdminLibrary.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.UsersModel
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController(
         AppDbContext context
        ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet("GetUsers")]
        public async Task<List<UsersDto>> GetUsers()
        {

            var listUsers = await _context.Users.Include(u => u.UsersRolesVirtual)
                                                    .ThenInclude(r => r.RolesVirtual).ToListAsync();

            if (listUsers.Count == 0)
            {
                return [];
            }

            return listUsers.Select(m => new UsersDto(
                                        m.FirtsName,
                                        m.MiddleName,
                                        m.FirtsLastName,
                                        m.SecondLastName,
                                        m.TypeIdentification,
                                        m.NumberIdentification,
                                        m.Status,
                                        m.UserName,
                                        m.UserType,
                                        m.UsersRolesVirtual?.Select(x => x.RolesVirtual.Name).ToList() ?? []
                                    )).ToList();
        }

        [HttpPost("Create")]
        public async Task<ResponseDto> CreateUser(
           UserControllerRequest userControllerRequest
           )
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(userControllerRequest.FirtsName) ||
                    string.IsNullOrEmpty(userControllerRequest.FirtsLastName) ||
                    string.IsNullOrEmpty(userControllerRequest.TypeIdentification) ||
                    string.IsNullOrEmpty(userControllerRequest.NumberIdentification)
                    )
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var isExist = await _context.Users.FirstOrDefaultAsync(x => x.NumberIdentification == userControllerRequest.NumberIdentification);

                if (isExist != null)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.exists);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                var users = new Users(userControllerRequest.FirtsName!,
                                      userControllerRequest.MiddleName,
                                      userControllerRequest.FirtsLastName,
                                      userControllerRequest.SecondLastName,
                                      userControllerRequest.TypeIdentification,
                                      userControllerRequest.NumberIdentification,
                                      userControllerRequest.Status,
                                      userControllerRequest.UserName,
                                      userControllerRequest.UserType!
                                      );

                await _context.Users.AddAsync(users);
                var result = await _context.SaveChangesAsync();

                if (userControllerRequest.Roles != null)
                {
                    var roleEntities = await _context.Roles.Where(r => userControllerRequest.Roles.ToLower().Contains(r.Name.ToLower()))
                                                          .ToListAsync();

                    var roleAssignments = roleEntities.Select(role => new UsersRoles
                    {
                        IdUser = users.Id,
                        IdRol = role.Id
                    }).ToList();

                    await _context.UsersRoles.AddRangeAsync(roleAssignments);
                    await _context.SaveChangesAsync();
                }

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
          UserControllerUpdate userControllerUpdate
          )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

                var isExist = await _context.Users.FirstOrDefaultAsync(x => x.Id == userControllerUpdate.Id);

                if (isExist != null)
                {
                    isExist.FirtsName = userControllerUpdate.FirtsName!;
                    isExist.MiddleName = userControllerUpdate.MiddleName!;
                    isExist.FirtsLastName = userControllerUpdate.FirtsLastName!;
                    isExist.SecondLastName = userControllerUpdate.SecondLastName!;
                    isExist.TypeIdentification = userControllerUpdate.TypeIdentification!;
                    isExist.NumberIdentification = userControllerUpdate.NumberIdentification!;
                    isExist.Status = userControllerUpdate.Status;
                    isExist.UserName = userControllerUpdate.UserName!;
                    
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "User";

                    _context.Users.Update(isExist);
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
