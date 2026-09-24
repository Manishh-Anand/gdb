using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Logging;
using Microsoft.Extensions.Logging;

namespace GDB.App.Infrastructure.Repositories
{
    public class DataBaseConnectionManager
    {
        private static readonly ILogger _logger = AppLogger.CreateLogger<DataBaseConnectionManager>();

        public static DbConnection GetConnection()
        {
            var settings = ConfigurationManager.ConnectionStrings["GDBConnection"];

            if (settings == null)
            {
                _logger.LogError("Connection string 'GDBConnection' is missing from App.config");
                throw new ConfigurationErrorsException("Connection string 'GDBConnection' not found.");
            }

            string connectionString = settings.ConnectionString;
            string providerName = settings.ProviderName;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();

                connection.ConnectionString = connectionString;

                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create DB connection for provider {ProviderName}", providerName);
                throw;
            }
        }
    }
}
