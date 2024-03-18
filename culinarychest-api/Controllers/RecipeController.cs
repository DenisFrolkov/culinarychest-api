using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

//// тот атрибут определяет маршрут для контроллера. Здесь [controller] является плейсхолдером,
// // который автоматически заменяется на имя контроллера без суффикса "Controller".
// // Это позволяет легко определить маршрут для каждого контроллера без необходимости явно указывать его в атрибуте. 
[Route("api/recipe")]
[ApiController] //Этот атрибут указывает, что класс является контроллером API
public class RecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public RecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IActionResult GetAllRecipes()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных.
        var recipes = _repository.Recipe.GetAllRecipes(trackChanges: false);
        var recipeDto = _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        //Использует AutoMapper для преобразования каждого объекта Recipe в объект RecipeDto. Результатом является коллекция объектов RecipeDto.
        return Ok(recipeDto);
    }
}