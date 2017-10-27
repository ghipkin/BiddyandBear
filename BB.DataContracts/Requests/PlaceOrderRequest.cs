using System.Runtime.Serialization;
using System.Collections.Generic;
using BB.DataLayer;

namespace BB.DataContracts
{
    public class PlaceOrderRequest
    {
        public Order NewOrder { get; set; }

        public List<OrderLine> OrderedItems {get; set;}

    }
}
