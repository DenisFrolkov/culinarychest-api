using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/{authorId}/favoriteRecipe")]
[ApiController]
public class ApplicationUserFavoriteRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger; 
    private readonly IMapper _mapper;

    
    public ApplicationUserFavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository; 
        _logger = logger; 
        _mapper = mapper;
    }

    [HttpGet(Name = "GetFavoriteRecipeForApplicationUserByAuthorId")]
    public async Task<IActionResult> GetApplicationUserFavoriteRecipes(int authorId, 
        [FromQuery] ApplicationUserFavoriteRecipeParameters applicationUserFavoriteRecipeParameters)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var dbFavoriteRecipe = await _repository.FavoriteRecipe.GetApplicationUserFavoriteRecipes(authorId, 
            applicationUserFavoriteRecipeParameters, trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbFavoriteRecipe.MetaData));
        var favoriteRecipeDto = _mapper.Map<IEnumerable<FavoriteRecipeDto>>(dbFavoriteRecipe);
        return Ok(favoriteRecipeDto);
    }
    
    [HttpDelete("{favoriteRecipeId}")]
    public async Task<IActionResult> DeleteApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        
        var applicationUserFavoriteRecipe =
             await _repository.FavoriteRecipe.GetApplicationUserFavoriteRecipe(authorId, favoriteRecipeId, trackChanges: false);
        if (applicationUserFavoriteRecipe == null)
        {
            _logger.LogInfo($"FavoriteRecipe with id: {favoriteRecipeId} doesn't exist in the database.");
            return NotFound();
        }
        _repository.FavoriteRecipe.DeleteFavoriteRecipe(applicationUserFavoriteRecipe);
        _repository.SaveAsync();
        return NoContent();
    }
}