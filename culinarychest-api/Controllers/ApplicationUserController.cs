using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public ApplicationUserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAppActionUser()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных.
        var applicationUsers = _repository.ApplicationUser.GetAllApplicationUsers(trackChanges: false);
        var applicationUserDto = _mapper.Map<IEnumerable<ApplicationUserDto>>(applicationUsers);
        //Использует AutoMapper для преобразования каждого объекта applicationUser в объект ApplicationUserDto. Результатом является коллекция объектов ApplicationUserDto.
        return Ok(applicationUserDto);
    }
}

