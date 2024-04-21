using System.Security.Claims;
using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[ApiVersion("1.0")]
[Route("api/applicationUser/recipe")]
[ApiController]
public class ApplicationUserRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;


    public ApplicationUserRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetApplicationUserRecipes([FromQuery] ApplicationUserRecipeParameters applicationUserRecipeParameters)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var dbRecipes = await _repository.Recipe.GetApplicationUserRecipesAsync(user.Id, applicationUserRecipeParameters, 
            trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipes.MetaData));
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipes);
        return Ok(recipesDto);
    }

    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUserRecipe([FromBody] CreateRecipeDto recipe)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var recipeEntity = _mapper.Map<Recipe>(recipe);
        recipeEntity.AuthorId = user.Id; 
        _repository.Recipe.CreateApplicationUserRecipe(user.Id, recipeEntity);
        await _repository.SaveAsync();
        var successMessage = "Recipe has been created successfully.";
        var recipeToReturn = _mapper.Map<RecipeDto>(recipeEntity);
        return CreatedAtRoute("GetApplicationUserRecipesByAuthorId", new
        {
            authorId = user.Id,
            id = recipeToReturn.RecipeId
        }, successMessage);
    }
    
    [HttpDelete("{recipeId}"), Authorize]
    public async Task<IActionResult> DeleteApplicationUserRecipe(int recipeId)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var applicationUserRecipe = 
            await _repository.Recipe.GetApplicationUserRecipeAsync(user.Id, recipeId, trackChanges: false);
        if (applicationUserRecipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        _repository.Recipe.DeleteRecipe(applicationUserRecipe);
        await _repository.SaveAsync();
        return NoContent();
    }

    [HttpPut("{recipeId}"), Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateApplicationUserRecipe(int recipeId, [FromBody] UpdateRecipeDto recipe)
    {
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