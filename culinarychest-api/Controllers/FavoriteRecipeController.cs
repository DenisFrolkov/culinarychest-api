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
        var favoriteRecipes = _repository.FavoriteRecipe.GetAllFavoriteRecipes(trackChanges: false);
        var favoriteRecipeDto = _mapper.Map<IEnumerable<FavoriteRecipeDto>>(favoriteRecipes);
        return Ok(favoriteRecipeDto);
    }
    
    [HttpGet("{favoriteRecipeId}")]
    public IActionResult GetFavoriteRecipe(int favoriteRecipeId)
    {
        var favoriteRecipe = _repository.FavoriteRecipe.GetFavoriteRecipe(favoriteRecipeId, trackChanges: false);
        if (favoriteRecipe == null)
        {
            _logger.LogInfo($"FavoriteRecipe with id: {favoriteRecipeId} doesn't exist in the database.");
            return NotFound();
        }        else
        {
            var favoriteRecipeDto = _mapper.Map<FavoriteRecipeDto>(favoriteRecipe);
            return Ok(favoriteRecipeDto);        
        }
    }
}