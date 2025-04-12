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
       
        private readonly AppDbContext _context = context;
        private readonly MaterialsMovements movements = new();
        private readonly MaterialHistory history = new();
     

        #region Lista de libros
        [HttpGet("GetLoans")]
        public async Task<List<MovementsDto>> GetMovements()
        {
            return await movements.ListMovements(_context);
        }
        #endregion

        #region Historial de materiales
        [HttpGet("history")]
        public async Task<List<MovementsDto>> History()
        {
            return await history.HistoryMovements(_context);
        }
        #endregion

        #region Prestar libros
        [HttpPost("Loans")]
        public async Task<ResponseDto> Movements(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            return await movements.Loans(_context, movementsControllerRequest);
        }
        #endregion

        #region Devolver libros
        [HttpDelete("return")]
        public async Task<ResponseDto> DeleteLoans(
            MovementsControllerRequest movementsControllerRequest
            )
        {
            return await movements.Return(_context, movementsControllerRequest);
        }
        #endregion

        
    }
}
