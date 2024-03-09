using Contracts;
using NLog;

namespace LoggerService;

public class LoggerManager : ILoggerManager //Класс LoggerManager реализует интерфейс ILoggerManager, который, предназначен для логирования различных типов сообщений (информационные, предупреждения, отладочные и сообщения об ошибках).
{
    private static ILogger logger = LogManager.GetCurrentClassLogger(); //Это статический экземпляр логгера, полученный из NLog. GetCurrentClassLogger() автоматически создает логгер для текущего класса, что упрощает логирование в контексте этого класса
    public LoggerManager() { }
    public void LogDebug(string message) 
    {
        logger.Debug(message); //Записывает отладочное сообщение
    }
    public void LogError(string message)
    {
        logger.Error(message); //Записывает отладочное сообщение
    }
    public void LogInfo(string message) {
        logger.Info(message); //Записывает информационное сообщение
    }
    public void LogWarn(string message)
    {
        logger.Warn(message); //Записывает предупреждение
    } 
}