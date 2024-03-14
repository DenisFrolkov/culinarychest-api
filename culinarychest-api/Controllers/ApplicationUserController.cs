using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

// тот атрибут определяет маршрут для контроллера. Здесь [controller] является плейсхолдером,
// который автоматически заменяется на имя контроллера без суффикса "Controller".
// Это позволяет легко опред елить маршрут для каждого контроллера без необходимости явно указывать его в атрибуте. 
[Route("api/applicationUser")]
[ApiController] //Этот атрибут указывает, что класс является контроллером API
public class ApplicationUserController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    
    public ApplicationUserController(IRepositoryManager repository, ILoggerManager logger) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAppActionUser()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных. Внутри метода используется блок try-catch
        //для обработки возможных исключений. Если запрос успешно обработан, метод возвращает статус 200 (OK) и данные шагов. 
        try
        {
            var applicationUser = _repository.ApplicationUser.GetAllApplicationUsers(trackChanges: false);
            var applicationUserDto = applicationUser.Select(user => new ApplicationUserDto 
                //преобразовывает информацию о пользователях в dto список
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password
                
            }).ToList();
            return Ok(applicationUserDto);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetAppActionUser)} action {ex}");
            return StatusCode(500, "Internal server error");
        }
    }
}

