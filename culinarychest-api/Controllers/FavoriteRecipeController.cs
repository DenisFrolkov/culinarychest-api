using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

//// тот атрибут определяет маршрут для контроллера. Здесь [controller] является плейсхолдером,
// // который автоматически заменяется на имя контроллера без суффикса "Controller".
// // Это позволяет легко определить маршрут для каждого контроллера без необходимости явно указывать его в атрибуте. 
[Route("api/favoriteRecipe")]
[ApiController] //Этот атрибут указывает, что класс является контроллером API
public class FavoriteRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public FavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IActionResult GetFavoriteRecipe()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных. 
        var favoriteRecipes = _repository.FavoriteRecipe.GetAllFavoriteRecipes(trackChanges: false);
        var favoriteRecipeDto = _mapper.Map<IEnumerable<FavoriteRecipeDto>>(favoriteRecipes);
        //Использует AutoMapper для преобразования каждого объекта favoriteRecipe в объект FavoriteRecipeDto. Результатом является коллекция объектов FavoriteRecipeDto.
        return Ok(favoriteRecipeDto);
    }
}