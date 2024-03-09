namespace Contracts;

public interface ILoggerManager //Этот интерфейс определяет базовые методы для записи различных типов сообщений в журнал
{
    void LogInfo(string message); //редназначен для записи информационных сообщений
    void LogWarn(string message); //используется для записи предупреждений
    void LogDebug(string message); //предназначен для записи отладочных сообщений
    void LogError(string message); //используется для записи сообщений об ошибках
}