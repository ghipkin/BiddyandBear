using System;
using System.Collections.Generic;
using BB.DataLayer.Abstract;

namespace BB.Mocks
{
    internal static class MockDatabase
    {
        internal static List<DatabaseRecord> MockedDb { get; set; }

        internal static void Insert(DatabaseRecord record)
        {
            if(MockedDb == null)
            {
                MockedDb = new List<DatabaseRecord>();
            }

            MockedDb.Add(record);
        }
    }
}
