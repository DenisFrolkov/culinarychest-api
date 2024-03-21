using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/recipe/{recipeId}/steps")]
public class StepsForRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    
    public StepsForRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(Name = "StepsForRecipeByRecipeId")]
    public IActionResult GetStepsForRecipe(int recipeId)
    {
        var recipe = _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepsFromDb = _repository.Step.GetRecipeForAllSteps(recipeId, trackChanges: false);
        var stepDto = _mapper.Map<IEnumerable<StepDto>>(stepsFromDb);
        return Ok(stepDto);
    }
}