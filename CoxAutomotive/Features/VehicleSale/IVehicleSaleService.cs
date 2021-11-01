using CoxAutomotive.Features.VehicleSale.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoxAutomotive.Features.VehicleSale
{
    public interface IVehicleSaleService
    {
        Task<ResponseModel<VehicleSaleResponseModel>> UploadFile(IFormFile file);
    }
}
