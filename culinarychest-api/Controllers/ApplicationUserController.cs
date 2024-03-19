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
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult GetApplicationUsers()
    {
        var applicationUsers = _repository.ApplicationUser.GetAllApplicationUsers(trackChanges: false);
        var applicationUserDto = _mapper.Map<IEnumerable<ApplicationUserDto>>(applicationUsers);
        return Ok(applicationUserDto);
    }
    
    [HttpGet("{userId}")]
    public IActionResult GetApplicationUser(int userId)
    {
        var applicationUser = _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {userId} doesn't exist in the database.");
            return NotFound();
        }
        else
        {
            var applicationUserDto = _mapper.Map<ApplicationUserDto>(applicationUser);
            return Ok(applicationUserDto);
        }
    }
}

