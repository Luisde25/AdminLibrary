using AdminLibrary.Constants;
using AdminLibrary.Controllers.UsersModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
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
        #region Instancia para acceder a la base de datos
        private readonly AppDbContext _context = context;
        #endregion

        #region Lista de usuarios que tienen un rol asignado
        [HttpGet("GetUsers")]
        public async Task<List<UsersDto>> GetUsers()
        {

            var listUsers = await _context.Users.Include(u => u.UsersRolesVirtual)
                                                    .ThenInclude(r => r.RolesVirtual).ToListAsync();

            if (listUsers.Count == 0)
            {
                return [];
            }

            var Users = listUsers.Select(m => new UsersDto(
                                        m.Id,
                                        m.FirtsName,
                                        m.MiddleName,
                                        m.FirtsLastName,
                                        m.SecondLastName,
                                        m.TypeIdentification,
                                        m.NumberIdentification,
                                        m.Status,
                                        m.UserName,
                                        m.UserType,
                                        m.UsersRolesVirtual?.FirstOrDefault()?.RolesVirtual.Name ?? string.Empty
                                    ));


            return Users.Where(x => !string.IsNullOrEmpty(x.RolName)).ToList();
        }
        #endregion

        #region Creación de un nuevo usuario
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
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.existsUser);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                var users = new Users(userControllerRequest.FirtsName!,
                                      userControllerRequest.MiddleName,
                                      userControllerRequest.FirtsLastName,
                                      userControllerRequest.SecondLastName ?? string.Empty,
                                      userControllerRequest.TypeIdentification,
                                      userControllerRequest.NumberIdentification,
                                      userControllerRequest.Status,
                                      userControllerRequest.UserName,
                                      userControllerRequest.UserType!
                                      );

                await _context.Users.AddAsync(users);
                var result = await _context.SaveChangesAsync();

                if (userControllerRequest.Rol != 0)
                {
                    var roleEntities = await _context.Roles.ToListAsync();

                    var roleAssignments = roleEntities.Select(role => new UsersRoles
                    {
                        IdUser = users.Id,
                        IdRol = userControllerRequest.Rol
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
                response.Message = failed?.Message + ex.Message;

            }
            return response;
        }

        #endregion

        #region Actualizar usuario existe
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateUser(
          UserControllerUpdate userControllerUpdate
          )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await _context.Users.FirstOrDefaultAsync(x => x.Id == userControllerUpdate.Id);
                var roleEntities = await _context.UsersRoles.FirstOrDefaultAsync(x => x.IdRol == userControllerUpdate.Rol);

                if (isExist != null && roleEntities != null)
                {


                    isExist.FirtsName = userControllerUpdate.FirtsName!;
                    isExist.MiddleName = userControllerUpdate.MiddleName!;
                    isExist.FirtsLastName = userControllerUpdate.FirtsLastName!;
                    isExist.SecondLastName = userControllerUpdate.SecondLastName!;
                    isExist.TypeIdentification = userControllerUpdate.TypeIdentification!;
                    isExist.NumberIdentification = userControllerUpdate.NumberIdentification!;
                    isExist.Status = userControllerUpdate.Status;
                    isExist.UserName = userControllerUpdate.UserName!;
                    roleEntities.IdRol = userControllerUpdate.Rol;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "Admin";

                    _context.Users.Update(isExist);
                    var result = await _context.SaveChangesAsync();

                    _context.UsersRoles.Update(roleEntities);
                    await _context.SaveChangesAsync();

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

        #region Eliminar usuario existente
        [HttpDelete("Remove")]
        public async Task<ResponseDto> DeleteUser( int id
       )
        {
            ResponseDto response = new();
            try
            {
              var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == id);

                if (user != null)
                {
                    
                    var loans = _context.Movements.Where(r => r.UserId == id).ToList();

                    if (loans.Count > 0)
                    {
                        var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoDelete);
                        response.Code = failed?.Code ?? string.Empty;
                        response.Message = failed?.Message;

                        return response;
                    }

                    _context.Users.Remove(user); 
                    await _context.SaveChangesAsync();

                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.DeleteSuccess);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }
                else
                {
                    var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);
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
