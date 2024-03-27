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
    public async Task<IActionResult> GetRecipeSteps(int recipeId)
    {
        var recipe = await _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepsFromDb = await _repository.Step.GetRecipeSteps(recipeId, trackChanges: false);
        var stepDto = _mapper.Map<IEnumerable<StepDto>>(stepsFromDb);
        return Ok(stepDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipeSteps(int recipeId, [FromBody] CreateStepsDto step)
    {
        if (step == null)
        {
            _logger.LogError("CreateStepsDto object sent from client is null.");
            return BadRequest("CreateStepsDto object is null");
        }

        var recipe = await _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the CreateStepsDto object");
            return UnprocessableEntity(ModelState);
        }

        var stepEntity = _mapper.Map<Step>(step);
        _repository.Step.CreateRecipeStep(recipeId, stepEntity);
        await _repository.SaveAsync();
        var stepToReturn = _mapper.Map<StepDto>(stepEntity);
        return CreatedAtRoute("GetRecipeStepsByRecipeId", new
        {
            recipeId, id = stepToReturn.StepId
        }, stepToReturn);
    }

    [HttpPut("{stepId}")]
    public async Task<IActionResult> UpdateRecipeStep(int recipeId, int stepId, [FromBody] UpdateStepDto step)
    {
        if (step == null)
        {
            _logger.LogError("UpdateStepDto object sent from client is null.");
            return BadRequest("UpdateStepDto object is null");
        }

        var recipe = await _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        var stepEntity = await _repository.Step.GetStep(stepId, trackChanges: true);
        if (stepEntity == null)
        {
            _logger.LogInfo($"Step with id: {stepId} doesn't exist in the database.");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the UpdateStepDto object");
            return UnprocessableEntity(ModelState);
        }

        _mapper.Map(step, stepEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}