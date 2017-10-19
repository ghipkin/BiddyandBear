using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DataLayer
{
    interface IDatabaseRecords
    {
        List<IDatabaseRecord> LoadRecords(Dictionary<String, object> WhereParams);
    }
}
