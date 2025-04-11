using AdminLibrary.Constants;
using AdminLibrary.Controllers.Materials.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using AdminLibrary.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.audioVisual
{

    [ApiController]
    [Route("api/Materials")]
    public class MaterialsController(
            AppDbContext context
            ) : Controller
    {
        private readonly AppDbContext _context = context;

        #region Lista de materiales
        [HttpGet("GetMaterials")]
        public async Task<List<MaterialDto>> GetMaterials()
        {
            var listMaterials = await _context.Materials.ToListAsync();

            if (listMaterials.Count == 0)
            {
                return [];
            }

            return listMaterials.Select(m => new MaterialDto(
                                        m.Id,
                                        m.Identifier,
                                        m.Title,
                                        m.RegisterDate.ToString("yyyyMMdd HH:mm:ss"),
                                        m.RegisterQuantity,
                                        m.CurrentQuantity
                                    )).ToList();
        }
        #endregion

        #region Registrar un nuevo libro
        [HttpPost("Register")]
        public async Task<ResponseDto> CreateMaterial(
            MaterialControllerRequest materialControllerRequest
            )
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(materialControllerRequest.Identifier) || 
                    string.IsNullOrEmpty(materialControllerRequest.Title) || 
                    materialControllerRequest.RegisterQuantity <= 0 
                    )
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;    
                }

                var isExist = await _context.Materials.FirstOrDefaultAsync(x => x.Identifier == materialControllerRequest.Identifier);

                if (isExist != null) 
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.ExistMaterial);

                    response.Code = success?.Code ?? string.Empty; 
                    response.Message = success?.Message;

                    return response;

                }

                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id== materialControllerRequest.userId);

                if (user == null)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoFound);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }


                var material = new MaterialsModel(
                    materialControllerRequest.Identifier,
                    materialControllerRequest.Title,
                    localTime,
                    materialControllerRequest.RegisterQuantity,
                    materialControllerRequest.RegisterQuantity
                    )
                {
                    MovementsVirtual =
                                        [
                                            new MaterialsMovements
                                            {
                                                MovementType = ConstantsApi.Register,
                                                Observations = materialControllerRequest.Observacion,
                                                MovementDate = localTime,
                                                UserId =materialControllerRequest.userId
                                            }
                                        ]
                };

                await _context.Materials.AddAsync(material);
                var result = await _context.SaveChangesAsync();

                var historyMaterial = new MaterialHistory(material.Id,
                                                        materialControllerRequest.userId,
                                                        ConstantsApi.Message,
                                                         ConstantsApi.Register, 
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


            }catch (Exception ex)
            {
                var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message + ex.Message;

            }

            return response;
        }
        #endregion

        #region Actualizar libro existente
        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMaterial(
            MaterialControllerUpdate materialControllerRequest
            )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialControllerRequest.Id);

                if (isExist != null)
                {
                    isExist.Title = materialControllerRequest.Title;
                    isExist.RegisterQuantity = materialControllerRequest.RegisterQuantity;
                    isExist.CurrentQuantity = materialControllerRequest.CurrentQuantity;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "Admin";

                    _context.Materials.Update(isExist);
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
                response.Message = failed?.Message + ex.Message;
            }

            return response;
        }
        #endregion
    }
}
