using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public StepController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public IActionResult GetAllSteps()
    {
        var steps = _repository.Step.GetAllSteps(trackChanges: false);
        var stepDto = _mapper.Map<IEnumerable<StepDto>>(steps);
        return Ok(stepDto);
    }
    
    [HttpGet("{stepId}")]
    public IActionResult GetAllSteps(int stepId)
    {
        var step = _repository.Step.GetStep(stepId, trackChanges: false);
        if (step == null)
        {
            _logger.LogInfo($"Recipe with id: {stepId} doesn't exist in the database.");
            return NotFound();
        }        else
        {
            var stepDto = _mapper.Map<StepDto>(step);
            return Ok(stepDto);            
        }

    }
}