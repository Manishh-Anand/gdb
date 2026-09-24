using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace gdb.Logging;

public static class ConfiguredLogger
{
    private static readonly ILoggerFactory _factory;

    static ConfiguredLogger()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _factory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole();
        });
    }

    public static ILogger CreateLogger(string categoryName) => _factory.CreateLogger(categoryName);

    public static ILogger<T> CreateLogger<T>() => _factory.CreateLogger<T>();
}
