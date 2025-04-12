using AdminLibrary.Constants;
using AdminLibrary.Controllers.RolesModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Shared;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Models.Entities
{
    public class Roles : EntityBase<int>
    {
        public Roles()
        {
            
        }
        public Roles(string name, string desciption, bool status)
        {
            Name = name;
            Description = desciption;
            Status = status;    
        }

        public string Name { get; set; }
        public string? Description { get; set; } 
        public bool Status { get; set; }

        /// <summary>
        /// Relacion de muchos a Uno.
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRolesVirtual { get; set; } = null!;

        public async Task<List<RolesDto>> ListRoles(AppDbContext context)
        {
            var listRoles = await context.Roles.ToListAsync();

            if (listRoles.Count == 0)
                return [];
            

            return listRoles.Select(m => new RolesDto(
                                        m.Id,
                                        m.Name,
                                        m.Description,
                                        m.Status
                                    )).ToList();
        }
        public async Task<ResponseDto> CreateNewRol(AppDbContext context, RoleControllerRequest roleControllerRequest)
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(roleControllerRequest.Name) ||
                    string.IsNullOrEmpty(roleControllerRequest.Description))
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var isExist = await context.Roles.FirstOrDefaultAsync(x => x.Name == roleControllerRequest.Name);

                if (isExist != null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.existsRol);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                var roles = new Roles(roleControllerRequest.Name,
                                      roleControllerRequest.Description,
                                      roleControllerRequest.status);

                await context.Roles.AddAsync(roles);
                var result = await context.SaveChangesAsync();

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

        public async Task<ResponseDto> UpdateCurrentRol(AppDbContext context, RoleControllerRequestUpdate roleControllerRequest)
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await context.Roles.FirstOrDefaultAsync(x => x.Id == roleControllerRequest.Id);

                if (isExist != null)
                {
                    isExist.Name = roleControllerRequest.Name;
                    isExist.Description = roleControllerRequest.Description;
                    isExist.Status = roleControllerRequest.status;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "Admin";

                    context.Roles.Update(isExist);
                    var result = await context.SaveChangesAsync();
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
    }
}
