using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/{authorId}/recipe")]
[ApiController]
public class ApplicationUserRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ApplicationUserRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetApplicationUserRecipesByAuthorId")]
    public async Task<IActionResult> GetApplicationUserRecipes(int authorId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipesDb = await _repository.Recipe.GetApplicationUserRecipes(authorId, trackChanges: false);
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(recipesDb);
        return Ok(recipesDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateApplicationUserRecipe(int authorId, [FromBody] CreateRecipeDto recipe)
    {
        if (recipe == null)
        {
            _logger.LogError("CreateRecipeDto object sent from client is null.");
            return BadRequest("CreateRecipeDto object is null"); 
        }

        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the CreateRecipeDto object");
            return UnprocessableEntity(ModelState);
        }

        var recipeEntity = _mapper.Map<Recipe>(recipe);
        _repository.Recipe.CreateApplicationUserRecipe(authorId, recipeEntity);
        await _repository.SaveAsync();
        var recipeToReturn = _mapper.Map<RecipeDto>(recipeEntity);
        return CreatedAtRoute("GetApplicationUserRecipesByAuthorId", new
        {
            authorId, id = recipeToReturn.RecipeId
        }, recipeToReturn);
    }

    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> DeleteApplicationUserRecipe(int authorId, int recipeId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var applicationUserRecipe = 
            await _repository.Recipe.GetApplicationUserRecipe(authorId, recipeId, trackChanges: false);
        if (applicationUserRecipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        _repository.Recipe.DeleteRecipe(applicationUserRecipe);
        await _repository.SaveAsync();
        return NoContent();
    }

    [HttpPut("{recipeId}")]
    public async Task<IActionResult> UpdateApplicationUserRecipe(int authorId, int recipeId, [FromBody] UpdateRecipeDto recipe)
    {
        if (recipe == null)
        {
            _logger.LogError("UpdateRecipeDto object sent from client is null.");
            return BadRequest("UpdateRecipeDto object is null");
        }

        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }

        var recipeEntity = await _repository.Recipe.GetRecipe(recipeId, trackChanges: true);
        if (recipeEntity == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the UpdateRecipeDto object");
            return UnprocessableEntity(ModelState);
        }

        _mapper.Map(recipe, recipeEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}