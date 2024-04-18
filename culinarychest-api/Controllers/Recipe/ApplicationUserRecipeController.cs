using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[ApiVersion("1.0")]
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
    public async Task<IActionResult> GetApplicationUserRecipes(string authorId, [FromQuery] ApplicationUserRecipeParameters applicationUserRecipeParameters)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var dbRecipes = await _repository.Recipe.GetApplicationUserRecipesAsync(authorId, applicationUserRecipeParameters, 
            trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipes.MetaData));
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipes);
        return Ok(recipesDto);
    }

    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUserRecipe(string authorId, [FromBody] CreateRecipeDto recipe)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipeEntity = _mapper.Map<Recipe>(recipe);
        _repository.Recipe.CreateApplicationUserRecipe(authorId, recipeEntity);
        await _repository.SaveAsync();
        var successMessage = $"Recipe has been created successfully.";
        var recipeToReturn = _mapper.Map<RecipeDto>(recipeEntity);
        return CreatedAtRoute("GetApplicationUserRecipesByAuthorId", new
        {
            authorId, id = recipeToReturn.RecipeId
        }, successMessage);
    }

    [HttpDelete("{recipeId}")]
    public async Task<IActionResult> DeleteApplicationUserRecipe(string authorId, int recipeId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var applicationUserRecipe = 
            await _repository.Recipe.GetApplicationUserRecipeAsync(authorId, recipeId, trackChanges: false);
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateApplicationUserRecipe(string authorId, int recipeId, [FromBody] UpdateRecipeDto recipe)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }

        var recipeEntity = await _repository.Recipe.GetRecipeAsync(recipeId, trackChanges: true);
        if (recipeEntity == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        _mapper.Map(recipe, recipeEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}