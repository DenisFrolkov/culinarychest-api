using System.Security.Claims;
using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[ApiVersion("1.0")]
[Route("api/1.0/applicationUser/{recipeId}")]
[ApiController]
public class CreateApplicationUserFavoriteRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public CreateApplicationUserFavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUserFavoriteRecipe(int recipeId, [FromBody] CreateFavoriteRecipeDtoDto favoriteRecipe)
    {
        var recipe = await _repository.Recipe.GetRecipeAsync(recipeId, trackChanges: false);
        if (recipe == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        
        var favoriteRecipeEntity = _mapper.Map<FavoriteRecipe>(favoriteRecipe);
        _repository.FavoriteRecipe.CreateApplicationUserFavoriteRecipe(user.Id, recipeId, favoriteRecipeEntity);
        await _repository.SaveAsync();
        var favoriteRecipeToReturn = _mapper.Map<FavoriteRecipeDto>(favoriteRecipeEntity);
        return CreatedAtRoute(new
        {
            user.Id, id = favoriteRecipeToReturn.FavoriteRecipeId
        }, favoriteRecipeToReturn);
    }
}