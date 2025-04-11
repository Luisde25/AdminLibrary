using AdminLibrary.Constants;
using AdminLibrary.Controllers.Movement.requests;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.Movement
{
    [ApiController]
    [Route("api/Materials")]
    public class MovementsController(
         AppDbContext context

        ) : Controller
    {

        #region Instancia para acceder a la base de datos
        private readonly AppDbContext _context = context;
        #endregion

        #region Lista de libros
        [HttpGet("GetLoans")]
        public async Task<List<MovementsDto>> GetMovements()
        {
            var listMovements = await (from m in _context.Movements
                                       join u in _context.Users on m.UserId equals u.Id
                                       join mt in _context.Materials on m.MaterialsId equals mt.Id
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
        #endregion

        #region Prestar libros
        [HttpPost("Loans")]
        public async Task<ResponseDto> Movements(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

            if (string.IsNullOrEmpty(movementsControllerRequest.MovementType))
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.MaterialId);

            if (material == null)
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.UserId);

            if (user == null)
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoFound);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var listMovements = await (from m in _context.Movements
                                       join u in _context.Users on m.UserId equals u.Id
                                       join mt in _context.Materials on m.MaterialsId equals mt.Id
                                       where m.MovementType == ConstantsApi.Loans
                                       select m).ToListAsync();
           
            if (listMovements.Select(x => x.MaterialsId).Count() > material.CurrentQuantity )
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.LimitMaterial);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }


            if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Student.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantStudent)
            {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxEstudents);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

            }
            else if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Teacher.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantTeacher)
            {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxProf);
                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
            }
            else if (movementsControllerRequest.MovementType.ToLower() == ConstantsApi.Loans.ToLower() && user.UserType.ToLower() == ConstantsApi.Admin.ToLower() && listMovements.Select(x => x.UserId).Count() > ConstantsApi.CantAdmin)
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxAdmin);
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

            await _context.Movements.AddAsync(movimiento);
           var result = await _context.SaveChangesAsync();

            var historyMaterial = new MaterialHistory(material.Id,
                                                        user.Id,
                                                        "Prestamo de libro de " + material.Title,
                                                        ConstantsApi.Loans,
                                                        localTime
                                                    );

            await _context.History.AddAsync(historyMaterial);
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

            return response;
        }
        #endregion

        #region Devolver libros
        [HttpDelete("return")]
        public async Task<ResponseDto> DeleteLoans(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

            if (movementsControllerRequest.UserId > 0 && movementsControllerRequest.MaterialId > 0)
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.UserId);
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == movementsControllerRequest.MaterialId);

            if (user != null && material != null)
            {
                var deleteMovement = await _context.Movements.FirstOrDefaultAsync(x => x.UserId == user.Id && x.MaterialsId == material.Id);

                if (deleteMovement != null)
                {
                     _context.Movements.Remove(deleteMovement);
                    await _context.SaveChangesAsync();

                    var historyMaterial = new MaterialHistory(material.Id,
                                                     user.Id,
                                                     "Se devuelve el libro de " + material.Title,
                                                     ConstantsApi.Return,
                                                     localTime
                                                 );

                    await _context.History.AddAsync(historyMaterial);
                    await _context.SaveChangesAsync();


                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.DeleteSuccess);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }
                else
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                }

            }
            else
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;
            }

            return response;

        }
        #endregion
    }
}
