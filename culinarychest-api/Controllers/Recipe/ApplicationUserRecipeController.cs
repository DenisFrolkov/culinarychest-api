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
    public IActionResult GetApplicationUserRecipes(int authorId)
    {
        var applicationUser = _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipesDb = _repository.Recipe.GetApplicationUserRecipes(authorId, trackChanges: false);
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(recipesDb);
        return Ok(recipesDto);
    }

    [HttpPost]
    public IActionResult CreateApplicationUserRecipe(int authorId, [FromBody] CreateRecipeDto recipe)
    {
        if (recipe == null)
        {
            _logger.LogError("CreateRecipeDto object sent from client is null.");
            return BadRequest("CreateRecipeDto object is null"); 
        }

        var applicationUser = _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }

        var recipeEntity = _mapper.Map<Recipe>(recipe);
        _repository.Recipe.CreateApplicationUserRecipe(authorId, recipeEntity);
        _repository.Save();
        var recipeToReturn = _mapper.Map<RecipeDto>(recipeEntity);
        return CreatedAtRoute("GetApplicationUserRecipesByAuthorId", new
        {
            authorId, id = recipeToReturn.RecipeId
        }, recipeToReturn);
    }
}