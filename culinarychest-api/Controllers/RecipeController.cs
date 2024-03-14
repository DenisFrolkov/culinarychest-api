using Contracts;
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

    public RecipeController(IRepositoryManager repository, ILoggerManager logger) //IRepositoryManager и ILoggerManager.
        //IRepositoryManager используется для доступа к данным, а ILoggerManager для логирования.
    {
        _repository = repository;
        _logger = logger;
    }

    public IActionResult GetAllRecipes()
    {
        //метод обрабатывает HTTP GET запросы и возвращает все шаги из базы данных. Внутри метода используется блок try-catch
        //для обработки возможных исключений. Если запрос успешно обработан, метод возвращает статус 200 (OK) и данные шагов. 
        try
        {
            var recipe = _repository.Recipe.GetAllRecipes(trackChanges: false);
            var recipeDto = recipe.Select(recipe => new Recipe
            {
                //преобразовывает информацию о пользователях в dto список
                Id = recipe.Id,
                AuthorId = recipe.AuthorId,
                Title = recipe.Title,
                RecipeImage = recipe.RecipeImage,
                Ingredients = recipe.Ingredients,
                Steps = recipe.Steps,
                CreationDate = recipe.CreationDate,
                PreparationTime = recipe.PreparationTime,
                SavedCount = recipe.SavedCount
            }).ToList();
            return Ok(recipeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetAllRecipes)} action {ex}");
            return StatusCode(500, "Internal server error");
        }
    }
}