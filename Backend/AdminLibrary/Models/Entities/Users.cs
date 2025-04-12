using AdminLibrary.Constants;
using AdminLibrary.Controllers.UsersModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Shared;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Models.Entities
{
    public class Users : EntityBase<int>
    {
        public Users()
        {
            
        }
        public Users(
            string firtsName,
            string? middleName,
            string firtsLastName,
            string secondLastName,
            string typeIdentification,
            string numberIdentification,
            bool status,
            string? userName,
            string userType
            )
        {
            FirtsName = firtsName;
            MiddleName = middleName;
            FirtsLastName = firtsLastName;
            SecondLastName = secondLastName;
            TypeIdentification = typeIdentification;
            NumberIdentification = numberIdentification;
            Status = status;
            UserName = userName;
            UserType = userType;
        }

        public string FirtsName { get; set; } 
        public string? MiddleName { get; set; } 
        public string FirtsLastName { get; set; }
        public string? SecondLastName { get; set; }
        public string TypeIdentification { get; set; } 
        public string NumberIdentification { get; set; } 
        public bool Status { get; set; }
        public string? UserName { get; set; }
        public string UserType { get; set; } 

        /// <summary>
        /// Relacion de muchos a Uno.
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRolesVirtual { get; set; } = null!;
        public virtual ICollection<MaterialsMovements> MovementsVirtual { get; set; } = null!;
        public virtual ICollection<MaterialHistory> HistoryVirtual { get; set; } = null!;


        public async Task<List<UsersDto>> CallingListUsers(AppDbContext context )
        {
            var listUsers = await context.Users.Include(u => u.UsersRolesVirtual)
                                                   .ThenInclude(r => r.RolesVirtual).ToListAsync();

            if (listUsers.Count == 0) 
                return [];
            
            var users = listUsers.Select(m => new UsersDto(
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


            return users.Where(x => !string.IsNullOrEmpty(x.RolName)).ToList();
        }
        public async Task<ResponseDto> CreateNewUser(AppDbContext context, UserControllerRequest userControllerRequest)
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
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                    return response;
                }

                var isExist = await context.Users.FirstOrDefaultAsync(x => x.NumberIdentification == userControllerRequest.NumberIdentification);

                if (isExist != null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.existsUser);

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

                await context.Users.AddAsync(users);
                var result = await context.SaveChangesAsync();

                if (userControllerRequest.Rol != 0)
                {
                    var roleEntities = await context.Roles.ToListAsync();

                    var roleAssignments = roleEntities.Select(role => new UsersRoles
                    {
                        IdUser = users.Id,
                        IdRol = userControllerRequest.Rol
                    }).ToList();

                    await context.UsersRoles.AddRangeAsync(roleAssignments);
                    await context.SaveChangesAsync();
                }

                if (result > 0)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.success);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }
                else
                {
                    var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                    response.Code = failed?.Code ?? string.Empty;
                    response.Message = failed?.Message;
                }


            }
            catch (Exception ex)
            {
                var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;

            }
            return response;
        }
        public async Task<ResponseDto> UpdateCurrentUser(AppDbContext context, UserControllerUpdate userControllerUpdate)
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await context.Users.FirstOrDefaultAsync(x => x.Id == userControllerUpdate.Id);
                var roleEntities = await context.UsersRoles.FirstOrDefaultAsync(x => x.IdRol == userControllerUpdate.Rol);

                if (isExist != null && roleEntities != null)
                {

                    isExist.Id  = userControllerUpdate.Id!;
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

                    context.Users.Update(isExist);
                    var result = await context.SaveChangesAsync();

                    context.UsersRoles.Update(roleEntities);
                    await context.SaveChangesAsync();

                    if (result > 0)
                    {
                        var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.success);
                        response.Code = success?.Code ?? string.Empty;
                        response.Message = success?.Message;
                    }
                    else
                    {
                        var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                        response.Code = failed?.Code ?? string.Empty;
                        response.Message = failed?.Message;
                    }
                }
                else
                {
                    var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.updateFailed);
                    response.Code = failed?.Code ?? string.Empty;
                    response.Message = failed?.Message;
                }
            }
            catch (Exception ex)
            {
                var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;
            }

            return response;
        }
        public async Task<ResponseDto> RemoveCurrentUser(AppDbContext context, int id)
        {
            ResponseDto response = new();
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(r => r.Id == id);
                
                if (user != null)
                {
                    var loans = context.Movements.Where(r => r.UserId == id).ToList();
                    var userRol = await context.UsersRoles.Where(x => x.IdUser == id).ToListAsync();
                    if (loans.Count > 0)
                    {
                        var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoDelete);
                        response.Code = failed?.Code ?? string.Empty;
                        response.Message = failed?.Message;

                        return response;
                    }


                    context.UsersRoles.RemoveRange(userRol);
                    await context.SaveChangesAsync();

                    context.Users.Remove(user);
                    await context.SaveChangesAsync();

                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.DeleteSuccess);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }
                else
                {
                    var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);
                    response.Code = failed?.Code ?? string.Empty;
                    response.Message = failed?.Message;
                }

            }
            catch (Exception ex)
            {
                var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;
            }

            return response;
        }
    }
}
