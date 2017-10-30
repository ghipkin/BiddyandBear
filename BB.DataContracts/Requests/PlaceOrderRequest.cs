using System.Runtime.Serialization;
using System.Collections.Generic;

namespace BB.DataContracts
{
    [DataContract]
    public class PlaceOrderRequest
    {
        public Order NewOrder { get; set; }

        public List<OrderLine> OrderedItems {get; set;}

    }
}
