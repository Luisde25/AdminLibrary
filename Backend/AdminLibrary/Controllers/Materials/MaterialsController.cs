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


        [HttpGet("GetMaterials")]
        public async Task<List<MaterialDto>> GetMaterials()
        {
            var listMaterials = await _context.Materials.ToListAsync();

            if (listMaterials.Count == 0)
            {
                return [];
            }

            return listMaterials.Select(m => new MaterialDto(
                                        m.Identifier,
                                        m.Title,
                                        m.RegisterDate.ToString("yyyyMMdd HH:mm:ss"),
                                        m.RegisterQuantity,
                                        m.CurrentQuantity
                                    )).ToList();
        }

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
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.exists);

                    response.Code = success?.Code ?? string.Empty; 
                    response.Message = success?.Message;

                    return response;

                }

                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

                var material = new MaterialsModel(
                    materialControllerRequest.Identifier,
                    materialControllerRequest.Title,
                    localTime,
                    materialControllerRequest.RegisterQuantity,
                    materialControllerRequest.RegisterQuantity
                    );

                await _context.Materials.AddAsync(material);
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


            }catch (Exception ex)
            {
                var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message;

            }


            return response;
        }


        [HttpPut("UpdateMaterials")]
        public async Task<ResponseDto> UpdateMaterial(
            MaterialControllerUpdate materialControllerRequest
            )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

                var isExist = await _context.Materials.FirstOrDefaultAsync(x => x.Identifier == materialControllerRequest.Identifier);

                if (isExist != null)
                {
                    isExist.Title = materialControllerRequest.Title;
                    isExist.RegisterQuantity = materialControllerRequest.RegisterQuantity;
                    isExist.CurrentQuantity = materialControllerRequest.CurrentQuantity;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "User";

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
                response.Message = failed?.Message;
            }

            return response;
        }



    }
}
