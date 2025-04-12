using AdminLibrary.Constants;
using AdminLibrary.Controllers.Movement.requests;
using AdminLibrary.Dtos;
using AdminLibrary.Shared;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Models.Entities
{
    public class MaterialsMovements : EntityBase<int>
    {
        public int MaterialsId { get; set; }
        public int UserId { get; set; }
        public string? Observations { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public DateTime? MovementDate { get; set; }
        public virtual MaterialsModel MaterialsVirtual { get; set; } = null!;
        public virtual Users UserVirtual { get; set; } = null!;

        public async Task<List<MovementsDto>> ListMovements(AppDbContext context)
        {
            var listMovements = await (from m in context.Movements
                                       join u in context.Users on m.UserId equals u.Id
                                       join mt in context.Materials on m.MaterialsId equals mt.Id
                                       select new MovementsDto(
                                           mt.Title,
                                           u.UserName!,
                                           m.Observations,
                                           m.MovementType,
                                           m.MovementDate
                                       )).ToListAsync();

            if (listMovements.Count == 0)
            {
                return [];
            }

            return listMovements;
        }
        
        public async Task<ResponseDto> Loans(AppDbContext context, MovementsControllerRequest movementsControllerRequest)
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);
            try
            {
                if (string.IsNullOrEmpty(movementsControllerRequest.MovementType))
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.MaterialId);

                if (material == null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var user = await context.Users.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.UserId);

                if (user == null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoFound);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var listMovements = await (from m in context.Movements
                                           join u in context.Users on m.UserId equals u.Id
                                           join mt in context.Materials on m.MaterialsId equals mt.Id
                                           where m.MovementType == ConstantsApi.Loans && u.Id == user.Id
                                           select m).ToListAsync();

                if (listMovements.Select(x => x.MaterialsId).Count() > material.CurrentQuantity)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.LimitMaterial);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }


                if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Student.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantStudent)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxEstudents);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }
                else if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Teacher.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantTeacher)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxProf);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }
                else if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Admin.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantAdmin)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxAdmin);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var movimiento = new MaterialsMovements
                {
                    MaterialsId = material.Id,
                    UserId = user.Id,
                    MovementType = movementsControllerRequest.MovementType,
                    MovementDate = localTime,
                    Observations = movementsControllerRequest.Observations
                };

                await context.Movements.AddAsync(movimiento);
                var result = await context.SaveChangesAsync();

                var historyMaterial = new MaterialHistory(material.Id,
                                                            user.Id,
                                                            "Prestamo de libro de " + material.Title,
                                                            ConstantsApi.Loans,
                                                            localTime
                                                        );

                await context.History.AddAsync(historyMaterial);
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
            catch (Exception ex)
            {
                var failed = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;
            }

            return response;
            
        }

        public async Task<ResponseDto> Return(AppDbContext context, MovementsControllerRequest movementsControllerRequest)
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);
            try
            {
                

                if (movementsControllerRequest.UserId == 0 && movementsControllerRequest.MaterialId == 0)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var user = await context.Users.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.UserId);
                var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.MaterialId);

                if (user != null && material != null)
                {
                    var deleteMovement = await context.Movements.FirstOrDefaultAsync(x => x.UserId == user.Id && x.MaterialsId == material.Id);

                    if (deleteMovement != null)
                    {
                        context.Movements.Remove(deleteMovement);
                        await context.SaveChangesAsync();

                        var historyMaterial = new MaterialHistory(material.Id,
                                                         user.Id,
                                                         "Se devuelve el libro de " + material.Title,
                                                         ConstantsApi.Return,
                                                         localTime
                                                     );

                        await context.History.AddAsync(historyMaterial);
                        await context.SaveChangesAsync();


                        var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MateriaReturn);

                        response.Code = success?.Code ?? string.Empty;
                        response.Message = success?.Message;
                    }
                    else
                    {
                        var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                        response.Code = success?.Code ?? string.Empty;
                        response.Message = success?.Message;
                    }

                }
                else
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
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
