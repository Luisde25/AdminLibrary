using AdminLibrary.Controllers.Materials.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers.audioVisual
{

    [ApiController]
    [Route("api/Materials")]
    public class MaterialsController(
            AppDbContext context
            ) : Controller
    {
        private readonly AppDbContext _context = context;
        private readonly MaterialsModel materials = new();

        #region Lista de materiales
        [HttpGet("GetMaterials")]
        public async Task<List<MaterialDto>> GetMaterials()
        {
            return await materials.ListMaterials(_context);
        }
        #endregion

        #region Registrar un nuevo libro
        [HttpPost("Register")]
        public async Task<ResponseDto> CreateMaterial(
            MaterialControllerRequest materialControllerRequest
            )
        {
            return await materials.RegisterNewMaterial(_context, materialControllerRequest);
        }
        #endregion

        #region Actualizar libro existente
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMaterial(
            MaterialControllerUpdate materialControllerRequest
            )
        {
            return await materials.UpdateCurrentNewMaterial(_context, materialControllerRequest);
        }
        #endregion
    }
}
