using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUserFavoriteRecipe(int authorId, int recipeId, [FromBody] CreateFavoriteRecipeDtoDto favoriteRecipe)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"Company with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipe = await _repository.Recipe.GetRecipeAsync(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        var favoriteRecipeEntity = _mapper.Map<FavoriteRecipe>(favoriteRecipe);
         _repository.FavoriteRecipe.CreateApplicationUserFavoriteRecipe(authorId, recipeId, favoriteRecipeEntity);
        await _repository.SaveAsync();
        var favoriteRecipeToReturn = _mapper.Map<FavoriteRecipeDto>(favoriteRecipeEntity);
        return CreatedAtRoute("GetFavoriteRecipeForApplicationUserByAuthorId", new
        {
            authorId, id = favoriteRecipeToReturn.FavoriteRecipeId
        }, favoriteRecipeToReturn);
    }
}