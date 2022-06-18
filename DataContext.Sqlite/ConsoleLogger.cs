using Microsoft.Extensions.Logging;
using static System.Console;

namespace DataContext.Sqlite;

public class ConsoleLoggerFactory : ILoggerFactory
{
    public void AddProvider(ILoggerProvider provider) { }

    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger();
    }

    public void Dispose() { }
}

public class ConsoleLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // to avoid overlogging, you can filter on the log level
        return logLevel switch
        {
            LogLevel.Trace => false,
            LogLevel.Information => false,
            LogLevel.None => false,
            LogLevel.Debug => true,
            LogLevel.Warning => true,
            LogLevel.Error => true,
            LogLevel.Critical => true,
            _ => true
        };
        ;
    }

    public void Log<TState>(LogLevel logLevel,
        EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (eventId.Id == 20100) // execute SQL statement
        {
            Write($"Level: {logLevel}, Event Id: {eventId.Id}");

            // only output the state or exception if it exists
            if (state != null)
            {
                Write($", State: {state}");
            }

            if (exception != null)
            {
                Write($", Exception: {exception.Message}");
            }
            WriteLine();
        }
    }
}
