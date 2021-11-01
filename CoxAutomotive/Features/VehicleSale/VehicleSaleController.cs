using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Features.VehicleSale
{
    
    public class VehicleSaleController: ApiController
    {

        private readonly IVehicleSaleService vehicleSale;
        private readonly ILogger<VehicleSaleController> logger;


        public VehicleSaleController(IVehicleSaleService vehicleSaleService, ILogger<VehicleSaleController> logger)
        {
            this.vehicleSale = vehicleSaleService;
            this.logger = logger;
        }
        

        [HttpPost]
        [DisableRequestSizeLimit]
        [Route(nameof(UploadFile))]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var result = await this.vehicleSale.UploadFile(file);
                if (result.IsSucceeded)
                {
                    this.logger.LogInformation($"A new file has been uploaded");
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {

                this.logger.LogInformation($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
    }
}
