using System;
using System.Collections.Generic;

namespace CoxAutomotive.Features.VehicleSale.Models
{
    [Serializable]
    public class VehicleSaleResponseModel
    {
        public IList<VehicleSaleModel> List { set; get; }
        public string MostOftenSoldVehicle { get; set; }
    }
}
