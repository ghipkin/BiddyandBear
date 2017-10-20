using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.DataLayer
{
    public interface IDatabaseRecord
    {
        void Save();

        void Load(Dictionary<string, object> parms);
    }
}
