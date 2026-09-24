using System.Data;
using GDB.App.Data;

namespace GDB.App.Infrastructure.Repositories
{
    public static class GDBInMemoryDataStore
    {
        public static DataSet DataSet { get; } =
            GDBInMemoryDB.CreateDataSet();
    }
}