using System.Runtime.Serialization;
using BB.DataLayer;

namespace BB.DataContracts
{
    public class PlaceOrderRequest
    {
        public Order NewOrder { get; set; }
    }
}
