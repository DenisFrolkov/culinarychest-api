using Contracts;
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

    public FavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
    }

    public IActionResult GetFavoriteRecipe()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных. Внутри метода используется блок try-catch
        //для обработки возможных исключений. Если запрос успешно обработан, метод возвращает статус 200 (OK) и данные шагов. 
        try
        {;
            var favoriteRecipe = _repository.FavoriteRecipe.GetAllFavoriteRecipes(trackChanges: false);
            var favoriteRecipeDto = favoriteRecipe.Select(favoriteRecipe => new FavoriteRecipe
            {
                //преобразовывает информацию о пользователях в dto список
                FavoriteRecipeId = favoriteRecipe.FavoriteRecipeId,
                RecipeId = favoriteRecipe.RecipeId,
                UserId = favoriteRecipe.UserId,
                AddedDate = favoriteRecipe.AddedDate
            }).ToList();
            return Ok(favoriteRecipeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetFavoriteRecipe)} action {ex}");
            return StatusCode(500, "Internal server error");
        }
    }
}