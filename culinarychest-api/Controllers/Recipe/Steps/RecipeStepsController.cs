using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Вывод всех шагов рецепта
    /// </summary>
    /// <returns> Список созданных шагов рецептов</returns>.
    [HttpGet, Authorize]
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
    
    /// <summary>
    /// Создать шаги рецепта
    /// </summary>
    /// <returns> Список шагов рецепта</returns>.
    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateRecipeSteps(int recipeId, CreateStepsDto step)
    {
        var recipe = await _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
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

    /// <summary>
    /// Изменить шаг рецепта
    /// </summary>
    /// <returns> Успешное изменение рецепта</returns>.
    [HttpPut("{stepId}"), Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateRecipeStep(int recipeId, int stepId, UpdateStepDto step)
    {
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
        _mapper.Map(step, stepEntity);
        await _repository.SaveAsync();
        return NoContent();
    }

    /// <summary>
    /// Удалить шаг рецепта
    /// </summary>
    /// <returns> Успешное удаление рецепта</returns>.
    [HttpDelete("{stepId}"), Authorize]
    public async Task<IActionResult> DeleteRecipeStep(int recipeId, int stepId, [FromQuery] RecipeParameters recipeParameters)
    {
        var recipe = await _repository.Recipe.GetRecipeAsync(recipeId, recipeParameters, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        var recipeStep = 
            await _repository.Step.GetStep(stepId, trackChanges: false);
        if (recipeStep == null)
        {
            _logger.LogInfo($"Step with id: {stepId} doesn't exist in the database.");
            return NotFound();
        }
        _repository.Step.DeleteStep(recipeStep);
        await _repository.SaveAsync();
        return NoContent();
    }
}