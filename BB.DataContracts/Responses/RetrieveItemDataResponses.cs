using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BB.DataContracts
{
    [DataContract]
    public class RetrieveItemsResponse : ServiceResponse
    {
        [DataMember]
        public List<CurrentItem> Items { get; set; }
    }

    [DataContract]
    public class RetrieveItemCategoriesResponse : ServiceResponse
    {
        [DataMember]
        public List<CurrentItemCategory> Categories { get; set; }
    }
}
