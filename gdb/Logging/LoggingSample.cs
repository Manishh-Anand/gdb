using Microsoft.Extensions.Logging;
using gdb.Logging;

namespace gdb;

public static class LoggingSample
{
    public static void Run()
    {
        TestConfiguredLogger();
        TestQuietCategory();
        TestAppLogger();
        TestFileLogger();
    }

    private static void TestConfiguredLogger()
    {
        // Category "gdb.LoggingSample.ConfiguredLoggerSample" is set to "Trace" in
        // appsettings.json, so this instance logs every level including Trace/Debug.
        var logger = ConfiguredLogger.CreateLogger("gdb.LoggingSample.ConfiguredLoggerSample");

        using (logger.BeginScope("TransactionId={TransactionId}", Guid.NewGuid()))
        {
            logger.LogTrace("ConfiguredLogger: trace message - low level diagnostic detail");
            logger.LogDebug("ConfiguredLogger: debug message - value={Value}", 42);
            logger.LogInformation("ConfiguredLogger: information message - account created successfully");
            logger.LogWarning("ConfiguredLogger: warning message - balance is running low");
            logger.LogError("ConfiguredLogger: error message - failed to process transaction");
            logger.LogCritical("ConfiguredLogger: critical message - database connection lost");

            try
            {
                throw new InvalidOperationException("Simulated exception for logging demo");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ConfiguredLogger: exception caught while handling request");
            }
        }
    }

    private static void TestQuietCategory()
    {
        // Category "gdb.LoggingSample.QuietSample" is set to "None" in appsettings.json,
        // so none of these calls should produce any output.
        var logger = ConfiguredLogger.CreateLogger("gdb.LoggingSample.QuietSample");

        logger.LogInformation("QuietSample: this should never be printed");
        logger.LogError("QuietSample: neither should this");
    }

    private static void TestAppLogger()
    {
        var logger = AppLogger.CreateLogger("AppLoggerSample");

        logger.LogTrace("AppLogger: trace message - entering method");
        logger.LogDebug("AppLogger: debug message - user id={UserId}", 101);
        logger.LogInformation("AppLogger: information message - transaction started");
        logger.LogWarning("AppLogger: warning message - retrying transaction");
        logger.LogError("AppLogger: error message - transaction failed");
        logger.LogCritical("AppLogger: critical message - system out of memory");

        try
        {
            throw new InvalidOperationException("Simulated exception for logging demo");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AppLogger: exception caught while handling request");
        }
    }

    private static void TestFileLogger()
    {
        FileLogger.Log("FileLogger: application started");
        FileLogger.Log("FileLogger: user logged in");
        FileLogger.Log("FileLogger: warning - suspicious activity detected");
        FileLogger.Log("FileLogger: error - unable to write to database");
        FileLogger.Log("FileLogger: application shutting down");
    }
}
