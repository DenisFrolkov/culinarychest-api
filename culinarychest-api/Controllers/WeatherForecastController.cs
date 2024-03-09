using Contracts;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")] //определяет маршрут для контроллера
[ApiController] //это атрибут, который указывает, что класс является контроллером API
public class WeatherForecastController : ControllerBase //класс WeatherForecastController наследуется от ControllerBase
{
    //В конструкторе контроллера WeatherForecastController используется внедрение зависимостей
    //для получения экземпляра ILoggerManager. Этот интерфейс, как мы обсуждали ранее,
    //предназначен для логирования различных типов сообщений (информационные, предупреждения,
    //отладочные и сообщения об ошибках).
    private ILoggerManager _logger;
    public WeatherForecastController(ILoggerManager logger)
    {
        _logger = logger;
    }
    [HttpGet] //это атрибут, указывающий, что метод Get должен обрабатывать HTTP GET запросы
    public IEnumerable<string> Get() //Внутри метода Get используется _logger для записи различных типов сообщений.
    {
        _logger.LogInfo("Вот информационное сообщение от нашего контроллера значений.");
        _logger.LogDebug("Вот отладочное сообщение от нашего контроллера значений.");
        _logger.LogWarn("Вот сообщение предупреждения от нашего контроллера значений.");
        _logger.LogError("Вот сообщение об ошибке от нашего контроллера значений.");
        return new string[] { "value1", "value2" }; //Метод возвращает массив строк, который будет сериализован в JSON и отправлен обратно клиенту в ответ на HTTP GET запрос
    }
}