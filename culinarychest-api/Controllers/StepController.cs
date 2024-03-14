using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

//// тот атрибут определяет маршрут для контроллера. Здесь [controller] является плейсхолдером,
// // который автоматически заменяется на имя контроллера без суффикса "Controller".
// // Это позволяет легко определить маршрут для каждого контроллера без необходимости явно указывать его в атрибуте. 
[Route("api/step")]
[ApiController] //Этот атрибут указывает, что класс является контроллером API
public class StepController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    
    public StepController(IRepositoryManager repository, ILoggerManager logger) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
    }

    public IActionResult GetAllSteps()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных. Внутри метода используется блок try-catch
        //для обработки возможных исключений. Если запрос успешно обработан, метод возвращает статус 200 (OK) и данные шагов. 
        try
        {
            var step = _repository.Step.GetAllSteps(trackChanges: false);
            var stepDto = step.Select(step => new StepDto
            {
                //преобразовывает информацию о пользователях в dto список
                StepId = step.StepId,
                Description = step.Description,
                Order = step.Order,
                RecipeId = step.RecipeId
            }).ToList();
            return Ok(stepDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetAllSteps)} action {ex}");
            return StatusCode(500, "Internal server error");
        }
    }
}