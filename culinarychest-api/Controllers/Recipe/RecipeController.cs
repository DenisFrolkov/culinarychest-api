using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/recipe")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public RecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> GetRecipes([FromQuery] RecipeParameters recipeParameters)
    {
        var dbRecipe = await _repository.Recipe.GetRecipesAsync(recipeParameters, trackChanges: false);
        var recipeDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipe);
        return Ok(recipeDto);
    }
    
    [HttpGet(template: "{recipeId}", Name = "RecipeByRecipeId")]
    public async Task<IActionResult> GetRecipe(int recipeId)
    {
        var recipe = await _repository.Recipe.GetRecipeAsync(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        else
        { 
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            return Ok(recipeDto);
        }
    }
}