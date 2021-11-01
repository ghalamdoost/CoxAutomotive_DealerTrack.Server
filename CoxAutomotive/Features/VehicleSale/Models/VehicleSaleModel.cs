using System;

namespace CoxAutomotive.Features.VehicleSale.Models
{
    
    public class VehicleSaleModel
    {
        public int DealNumber { get; set; }
        public string CustomerName { get; set; }
        public string DealershipName { get; set; }
        public string Vehicle { get; set; }
        public decimal Price { set; get; }
        public DateTime Date { set; get; }
    }
}
