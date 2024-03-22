using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/recipe/{recipeId}/steps")]
public class RecipeStepsController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    
    public RecipeStepsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetRecipeStepsByRecipeId")]
    public IActionResult GetRecipeSteps(int recipeId)
    {
        var recipe = _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepsFromDb = _repository.Step.GetRecipeForSteps(recipeId, trackChanges: false);
        var stepDto = _mapper.Map<IEnumerable<StepDto>>(stepsFromDb);
        return Ok(stepDto);
    }
}