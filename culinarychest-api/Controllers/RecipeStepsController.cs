using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/recipe/{recipeId}/steps")]
[ApiController]
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

        var stepsFromDb = _repository.Step.GetRecipeSteps(recipeId, trackChanges: false);
        var stepDto = _mapper.Map<IEnumerable<StepDto>>(stepsFromDb);
        return Ok(stepDto);
    }

    [HttpPost]
    public IActionResult CreateRecipeSteps(int recipeId, [FromBody] CreateStepsDto step)
    {
        if (step == null)
        {
            _logger.LogError("CreateStepsDto object sent from client is null.");
            return BadRequest("CreateStepsDto object is null");
        }

        var recipe = _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepEntity = _mapper.Map<Step>(step);
        _repository.Step.CreateRecipeStep(recipeId, stepEntity);
        _repository.Save();
        var stepToReturn = _mapper.Map<StepDto>(stepEntity);
        return CreatedAtRoute("GetRecipeStepsByRecipeId", new
        {
            recipeId, id = stepToReturn.StepId
        }, stepToReturn);
    }

    [HttpPut("{stepId}")]
    public IActionResult UpdateRecipeStep(int recipeId, int stepId, [FromBody] UpdateStepDto step)
    {
        if (step == null)
        {
            _logger.LogError("UpdateStepDto object sent from client is null.");
            return BadRequest("UpdateStepDto object is null");
        }

        var recipe = _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepEntity = _repository.Step.GetStep(stepId, trackChanges: true);
        if (stepEntity == null)
        {
            _logger.LogInfo($"Step with id: {stepId} doesn't exist in the database.");
            return NotFound();
        }

        _mapper.Map(step, stepEntity);
        _repository.Save();
        return NoContent();
    }
}