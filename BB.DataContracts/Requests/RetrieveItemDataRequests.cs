using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DataContracts
{
    public class RetrieveItemCategoriesRequest
    {
    }

    public class RetrieveItemsRequest
    {
        public long CategoryId { get; set; }
    }
}
