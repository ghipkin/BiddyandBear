using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DataLayer
{
    interface IDatabaseRecords
    {
        void LoadRecords(Dictionary<String, object> WhereParams);
    }
}
