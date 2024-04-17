using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[ApiVersion("1.0")]
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
    
    [HttpGet(Name = "GetRecipes"), Authorize]
    public async Task<IActionResult> GetRecipes([FromQuery] RecipeParameters recipeParameters)
    {
        var dbRecipe = await _repository.Recipe.GetRecipesAsync(recipeParameters, trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipe.MetaData));
        var recipeDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipe);
        return Ok(_dataShaper.ShapeData(recipeDto, recipeParameters.Fields));    }
    
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