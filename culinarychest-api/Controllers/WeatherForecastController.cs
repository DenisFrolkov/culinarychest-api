using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("[controller]")] //определяет маршрут для контроллера
[ApiController] //это атрибут, который указывает, что класс является контроллером API
public class WeatherForecastController : ControllerBase //класс WeatherForecastController наследуется от ControllerBase
{
    private readonly IRepositoryManager _repository;
    private ApplicationUser _applicationUser;
    private FavoriteRecipe _favoriteRecipe;
    private Recipe _recipe;
    private Step _step;
    
    public WeatherForecastController(IRepositoryManager repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> Get() 
        //Метод Get возвращает массив строк. В данном примере, метод выполняет операции создания, обновления и удаления
        //сущностей через репозиторий, но результаты этих операций не используются.
    {
        _repository.Save();
        
        return new string[] { "value1", "value2" };
    }
}