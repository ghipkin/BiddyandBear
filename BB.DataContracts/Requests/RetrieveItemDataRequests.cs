using System;
using System.Runtime.Serialization;

namespace BB.DataContracts
{
    [DataContract]
    public class RetrieveItemCategoriesRequest
    {
    }
    [DataContract]
    public class RetrieveItemsRequest
    {
        public long CategoryId { get; set; }
    }
}
