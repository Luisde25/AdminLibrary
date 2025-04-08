using AdminLibrary.Controllers.MenusModel.request;
using AdminLibrary.Dtos;
using AdminLibrary.Models;
using AdminLibrary.Models.Entities;
using AdminLibrary.Models.Shared;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers.MenusModel
{
    [ApiController]
    [Route("api/Menus")]
    public class MenusController(
           AppDbContext context
        ) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpGet("GetMenus")]
        public async Task<List<MenusDto>> GetMenus()
        {
           
            var listMenus = await _context.Menus.ToListAsync();

            if (listMenus.Count == 0)
            {
                return [];
            }

            return listMenus.Select(m => new MenusDto(
                                        m.Name,
                                        m.Url,
                                        m.Father,
                                        m.Order,
                                        m.Status
                                    )).ToList();
            
        }

        [HttpPost("Create")]
        public async Task<ResponseDto> CreateMenu(
         MenuControllerRequest menuControllerRequest
         )
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(menuControllerRequest.Name) ||
                    string.IsNullOrEmpty(menuControllerRequest.Url) ||
                    menuControllerRequest.Order > 0
                    )
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var isExist = await _context.Menus.FirstOrDefaultAsync(x => x.Url == menuControllerRequest.Url);

                if (isExist != null)
                {
                    var success = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.exists);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                var menus = new Menus(menuControllerRequest.Name!,
                                      menuControllerRequest.Url,
                                      menuControllerRequest.Father,
                                      menuControllerRequest.Order,
                                      menuControllerRequest.Status
                                      );

                await _context.Menus.AddAsync(menus);
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
            catch (Exception ex)
            {
                var failed = await _context.Response.FirstOrDefaultAsync(r => r.Code == Codes.failed);
                response.Code = failed?.Code ?? string.Empty;
                response.Message = failed?.Message;

            }
            return response;
        }

        [HttpPut("Update")]
        public async Task<ResponseDto> UpdateMenu(
          MenuControllerUpdate menuControllerUpdate
          )
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(Constants.utcNow, Constants.timeZoneInfo);

                var isExist = await _context.Menus.FirstOrDefaultAsync(x => x.Id == menuControllerUpdate.Id);

                if (isExist != null)
                {
                    isExist.Name = menuControllerUpdate.Name!;
                    isExist.Url = menuControllerUpdate.Url!;
                    isExist.Father = menuControllerUpdate.Father!;
                    isExist.Order = menuControllerUpdate.Order!;
                    isExist.Status = menuControllerUpdate.Status!;
               
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "User";

                    _context.Menus.Update(isExist);
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
