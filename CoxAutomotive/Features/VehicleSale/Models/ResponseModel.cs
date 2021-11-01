using System;
using System.Collections.Generic;

namespace CoxAutomotive.Features.VehicleSale.Models
{
    [Serializable]
    public class ResponseModel<T>
    {
        public bool IsSucceeded { set; get; } = false;
        public int ResponseCode { set; get; }
        public List<string> ResponseMessage { set; get; } = new List<string>();
        public T Result { set; get; }
    }
}
