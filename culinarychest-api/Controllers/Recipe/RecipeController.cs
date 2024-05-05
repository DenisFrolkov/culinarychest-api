using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<RecipeDto> _dataShaper;

    public RecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<RecipeDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }
    
    /// <summary>
    /// Вывод всех рецептов
    /// </summary>
    /// <returns> Список рецептов</returns>.
    [HttpGet("listRecipe"), Authorize]
    public async Task<IActionResult> GetRecipes([FromQuery] RecipeParameters recipeParameters)
    {
        var dbRecipe = await _repository.Recipe.GetRecipesAsync(recipeParameters, trackChanges: false);
        foreach (var recipe in dbRecipe)
        {
            recipe.Steps = await _repository.Step.GetRecipeSteps(recipe.RecipeId, trackChanges: false);
        }
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipe.MetaData));
        var recipeDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipe);
        return Ok(_dataShaper.ShapeData(recipeDto, recipeParameters.Fields));
    }
    
    /// <summary>
    /// Вывод рецептов по их recipeId
    /// </summary>
    /// <returns> Рецепты с конкретными recipeId </returns>.
    [HttpGet("listRecipeByIds"), Authorize]
    public async Task<IActionResult> GetRecipeByRecipeId([FromQuery] List<int> recipeIds, [FromQuery] RecipeParameters recipeParameters)
    {
        var dbRecipes = await _repository.Recipe.GetRecipeByIdsAsync(recipeIds, recipeParameters, trackChanges: false);
    
        if (dbRecipes == null)
        {
            _logger.LogInfo($"Recipes with ids: {string.Join(",", recipeIds)} don't exist in the database.");
            return NotFound();
        }
    
        foreach (var recipe in dbRecipes)
        {
            recipe.Steps = await _repository.Step.GetRecipeSteps(recipe.RecipeId, trackChanges: false);
        }
    
        // Здесь можно добавить пагинацию, если это необходимо
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipes.MetaData));

        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipes);
        return Ok(recipesDto);
    }
    
    /// <summary>
    /// Вывод рецепта по ID
    /// </summary>
    /// <returns> Рецепт с конкретным ID </returns>.
    [HttpGet(template: "{recipeId}"), Authorize]
    public async Task<IActionResult> GetRecipeByIds(int recipeId, [FromQuery] RecipeParameters recipeParameters)
    {
        var dbRecipes = await _repository.Recipe.GetRecipeByIdsAsync(recipeId, recipeParameters, trackChanges: false);
        
        if (dbRecipes == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        
        foreach (var recipe in dbRecipes)
        {
            recipe.Steps = await _repository.Step.GetRecipeSteps(recipe.RecipeId, trackChanges: false);
        }
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipes.MetaData));
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipes);
        return Ok(recipesDto);
    }

}