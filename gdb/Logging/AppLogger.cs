using Microsoft.Extensions.Logging;
using Serilog;

namespace gdb.Logging;

public static class AppLogger
{
     private static readonly ILoggerFactory _factory;

    static AppLogger()
    {
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();

        _factory = LoggerFactory.Create(builder => builder.AddSerilog(serilogLogger, dispose: true));
    }

    public static Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) => _factory.CreateLogger(categoryName);

    public static ILogger<T> CreateLogger<T>() => _factory.CreateLogger<T>();
}
