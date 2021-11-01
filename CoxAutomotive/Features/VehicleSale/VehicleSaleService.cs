using CoxAutomotive.Features.VehicleSale.Models;
using CoxAutomotive.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Features.VehicleSale
{
    public class VehicleSaleService : IVehicleSaleService
    {

        private readonly ICsvSerializer<VehicleSaleModel> serializer;
        private readonly IFileValidator fileValidator;

        public VehicleSaleService(ICsvSerializer<VehicleSaleModel> serializer, IFileValidator fileValidator)
        {
            this.fileValidator = fileValidator;

            this.serializer = serializer;
            this.serializer.UseLineNumbers = false;
            this.serializer.UseTextQualifier = true;
        }

        
        public async Task<ResponseModel<VehicleSaleResponseModel>> UploadFile(IFormFile file)
        {
            try
            {
                var response = new ResponseModel<VehicleSaleResponseModel>();

                response.ResponseMessage = this.fileValidator.Validate(file);
                if (response.ResponseMessage.Count > 0)
                {
                    response.IsSucceeded = false;
                    response.ResponseCode = 400;
                    return response;
                }

                //convert to IList
                var list = await this.serializer.DeserializeAsync(file.OpenReadStream());

                //find the mostOftenSoldVehicle
                var mostOftenSoldVehicle = list.GroupBy(c => c.Vehicle)
                    .OrderByDescending(gp => gp.Count())
                    .Take(1)
                    .Select(g => g.Key).FirstOrDefault();


                response.IsSucceeded = true;
                response.Result = new VehicleSaleResponseModel
                {
                    List = list,
                    MostOftenSoldVehicle = mostOftenSoldVehicle
                };
                response.ResponseCode = 200;

                //Uploaded Successfully
                return response;

            }
            catch (Exception ex)
            {
                return new ResponseModel<VehicleSaleResponseModel> { IsSucceeded=false, ResponseCode=500, ResponseMessage = { $"Internal server error: {ex}" } , Result=null};

            }
        }
    }
}
