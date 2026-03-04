using MelonLoader;

namespace Amalgamuse.Utils;

internal class Logger
{
    private readonly MelonLogger.Instance _logger;

    internal Logger(string className)
    {
        _logger = new MelonLogger.Instance(className);
    }

    internal void Msg(string message, bool isDebug = true)
    {
        if (isDebug && !Preferences.Debug) return;
        _logger.Msg(message);
    }

    internal void Success(string message)
    {
        _logger.Msg(ConsoleColor.Green, "Success: " + message);
    }

    internal void Warning(string message)
    {
        _logger.Msg(ConsoleColor.Yellow, "Warning: " + message);
    }

    internal void Fail(string message)
    {
        _logger.Msg(ConsoleColor.Red, "FAILED: " + message);
    }

    internal void Error(string message)
    {
        _logger.Msg(ConsoleColor.Red, "ERROR: " + message);
    }
}
