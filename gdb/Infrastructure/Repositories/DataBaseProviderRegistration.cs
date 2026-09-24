using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Logging;
using Microsoft.Extensions.Logging;

namespace GDB.App.Infrastructure.Repositories
{
    public class DataBaseProviderRegistration
    {
        private static readonly ILogger _logger = AppLogger.CreateLogger<DataBaseProviderRegistration>();

        public static void Register()
        {
            try
            {
                RegisterProvider();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register DB provider factory");
                throw;
            }
        }

        private static void RegisterProvider()
        {
            string providerName =
            ConfigurationManager
                .ConnectionStrings["GDBConnection"]
                .ProviderName;

            string factoryTypeName =
                ConfigurationManager.AppSettings["ProviderFactory"];

            Type factoryType =
                Type.GetType(factoryTypeName);

            if (factoryType == null)
            {
                _logger.LogError("Provider factory type {FactoryTypeName} could not be loaded", factoryTypeName);
                throw new InvalidOperationException("Provider factory type not found: " + factoryTypeName);
            }

            var instanceField =
                factoryType.GetField(
                    "Instance",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Static);

            DbProviderFactory factory =
                (DbProviderFactory)instanceField.GetValue(null);

            DbProviderFactories.RegisterFactory(
                providerName,
                factory);

            _logger.LogInformation("Registered DB provider {ProviderName}", providerName);
        }

    }
}
