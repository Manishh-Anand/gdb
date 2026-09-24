namespace gdb.Logging;

public static class FileLogger
{
    private const string LogFilePath = "log-from-file.txt";

    public static void Log(string message)
    {
        File.AppendAllText(LogFilePath, DateTime.Now +"  "+ message + Environment.NewLine);
    }
}
