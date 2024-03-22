using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/{authorId}/{recipeId}")]
[ApiController]
public class CreateApplicationUserFavoriteRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CreateApplicationUserFavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpPost]
    public IActionResult CreateApplicationUserFavoriteRecipe(int authorId, int recipeId, [FromBody] CreateFavoriteRecipeDto favoriteRecipe)
    {
        if (favoriteRecipe == null)
        {
            _logger.LogError("CreateFavoriteRecipeDto object sent from client is null.");
            return BadRequest("CreateFavoriteRecipeDto object is null"); 
        }

        var applicationUser = _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"Company with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipe = _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        
        var favoriteRecipeEntity = _mapper.Map<FavoriteRecipe>(favoriteRecipe);
        _repository.FavoriteRecipe.CreateApplicationUserFavoriteRecipe(authorId, recipeId, favoriteRecipeEntity);
        _repository.Save();
        var favoriteRecipeToReturn = _mapper.Map<FavoriteRecipeDto>(favoriteRecipeEntity);
        return CreatedAtRoute("GetFavoriteRecipeForApplicationUserByAuthorId", new
        {
            authorId, id = favoriteRecipeToReturn.FavoriteRecipeId
        }, favoriteRecipeToReturn);
    }
}