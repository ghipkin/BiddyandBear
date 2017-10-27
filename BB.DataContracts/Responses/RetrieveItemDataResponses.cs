using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DataContracts
{
    public class RetrieveItemsResponse : ServiceResponse
    {
        public List<CurrentItem> Items { get; set; }
    }

    public class RetrieveItemCategoriesResponse : ServiceResponse
    {
        public List<CurrentItemCategory> Categories { get; set; }
    }
}
