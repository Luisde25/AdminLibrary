using AdminLibrary.Controllers.MenusModel.request;
using AdminLibrary.Controllers.Movement.requests;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using AdminLibrary.Models.Shared;
using Azure.Core;
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
        private readonly AppDbContext _context = context;

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

        [HttpPost("Loans")]
        public async Task<ResponseDto> Movements(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

            if (string.IsNullOrEmpty(movementsControllerRequest.MovementType))
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var material = await _context.Materials
                 .FirstOrDefaultAsync(m => m.Identifier == movementsControllerRequest.Identifier);

            if (material == null)
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaterialNoFound);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var user = await _context.Users
             .FirstOrDefaultAsync(m => m.UserName!.ToLower() == movementsControllerRequest.UserName!.ToLower());

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
                                       where m.MovementType == "prestamo" && u.Id == user.Id
                                       select m).ToListAsync();
           
            if (listMovements.Select(x => x.MaterialsId).Count() > material.CurrentQuantity )
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.LimitMaterial);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }


            if (movementsControllerRequest.MovementType.ToLower() == "prestamo" && user.UserType.ToLower() == "estudiante")
            {
               
                if (listMovements.Select(x => x.UserId).Count() > 5)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxEstudents);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

            }
            else if (movementsControllerRequest.MovementType.ToLower() == "prestamo" && user.UserType.ToLower() == "academico")
            {

                if (listMovements.Select(x => x.UserId).Count() > 1)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxProf);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }
            }
            else if (movementsControllerRequest.MovementType.ToLower() == "prestamo" && user.UserType.ToLower() == "profesor")
            {


                if (listMovements.Select(x => x.UserId).Count() > 3)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.MaxAdmin);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;
                      
                   return response;

                }
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


        [HttpDelete("return")]
        public async Task<ResponseDto> DeleteLoans(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            ResponseDto response = new();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

            if (string.IsNullOrEmpty(movementsControllerRequest.UserName) && string.IsNullOrEmpty(movementsControllerRequest.Identifier))
            {
                var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                response.Code = success?.Code ?? string.Empty;
                response.Message = success?.Message;

                return response;
            }

            var user = await _context.Users
            .FirstOrDefaultAsync(m => m.UserName!.ToLower() == movementsControllerRequest.UserName!.ToLower());

             var material = await _context.Materials
                 .FirstOrDefaultAsync(m => m.Identifier == movementsControllerRequest.Identifier);


            if (user != null && material != null)
            {
                var deleteMovement = await _context.Movements.FirstOrDefaultAsync(x => x.UserId == user.Id && x.MaterialsId == material.Id);

                if (deleteMovement != null)
                {
                     _context.Movements.Remove(deleteMovement);
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

    }
}
