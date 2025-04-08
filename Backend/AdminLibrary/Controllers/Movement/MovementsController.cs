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
    [Route("api/Movements")]
    public class MovementsController(
         AppDbContext context

        ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet("GetMovements")]
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

        [HttpPost("movements")]
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

    }
}
