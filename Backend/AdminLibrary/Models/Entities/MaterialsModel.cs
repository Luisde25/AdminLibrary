using AdminLibrary.Constants;
using AdminLibrary.Controllers.Materials.request;
using AdminLibrary.Dtos;
using AdminLibrary.Shared;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Models.Entities
{
    public class MaterialsModel : EntityBase<int>
    {
        public MaterialsModel()
        {

        }
        public MaterialsModel(
            string identifier, 
            string title, 
            DateTime registerDate,
            int registerQuantity,
            int currentQuantity
            )
        {
            Identifier = identifier; 
            Title = title;
            RegisterDate = registerDate;
            RegisterQuantity = registerQuantity;
            CurrentQuantity = currentQuantity;
        }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public DateTime RegisterDate { get; set; }
        public int RegisterQuantity { get; set; } 
        public int CurrentQuantity { get; set; }
        public virtual ICollection<MaterialsMovements> MovementsVirtual { get; set; } = null!;
        public virtual ICollection<MaterialHistory> HistoryVirtual { get; set; } = null!;

        public async Task<List<MaterialDto>> ListMaterials(AppDbContext context)
        {
            var listMaterials = await context.Materials.ToListAsync();

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
        public async Task<ResponseDto> RegisterNewMaterial(AppDbContext context, MaterialControllerRequest materialControllerRequest)
        {
            ResponseDto response = new();
            try
            {
                if (string.IsNullOrEmpty(materialControllerRequest.Identifier) ||
                    string.IsNullOrEmpty(materialControllerRequest.Title) ||
                    materialControllerRequest.RegisterQuantity <= 0
                    )
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.requestInvalid);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;
                }

                var isExist = await context.Materials.FirstOrDefaultAsync(x => x.Identifier == materialControllerRequest.Identifier);

                if (isExist != null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.ExistMaterial);

                    response.Code = success?.Code ?? string.Empty;
                    response.Message = success?.Message;

                    return response;

                }

                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == materialControllerRequest.userId);

                if (user == null)
                {
                    var success = await context.Response.FirstOrDefaultAsync(r => r.Code == Codes.UserNoFound);

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
                    );

                await context.Materials.AddAsync(material);
                var result = await context.SaveChangesAsync();

                var historyMaterial = new MaterialHistory(material.Id,
                                                        materialControllerRequest.userId,
                                                        ConstantsApi.Message,
                                                         ConstantsApi.Register,
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
        public async Task<ResponseDto> UpdateCurrentNewMaterial(AppDbContext context, MaterialControllerUpdate materialControllerRequest)
        {
            ResponseDto response = new();
            try
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ConstantsApi.utcNow, ConstantsApi.timeZoneInfo);

                var isExist = await context.Materials.FirstOrDefaultAsync(x => x.Id == materialControllerRequest.Id);

                if (isExist != null)
                {
                    isExist.Title = materialControllerRequest.Title;
                    isExist.RegisterQuantity = materialControllerRequest.RegisterQuantity;
                    isExist.CurrentQuantity = materialControllerRequest.CurrentQuantity;
                    isExist.UpdateDate = localTime;
                    isExist.UpdateUser = "Admin";

                    context.Materials.Update(isExist);
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
