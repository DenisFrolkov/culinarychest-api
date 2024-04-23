using System.Security.Claims;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/favoriteRecipe")]
[ApiController]
public class ApplicationUserFavoriteRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger; 
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ApplicationUserFavoriteRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetApplicationUserFavoriteRecipes( [FromQuery] ApplicationUserFavoriteRecipeParameters applicationUserFavoriteRecipeParameters)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var dbFavoriteRecipe = await _repository.FavoriteRecipe.GetApplicationUserFavoriteRecipes(user.Id, 
            applicationUserFavoriteRecipeParameters, trackChanges: false);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbFavoriteRecipe.MetaData));
        var favoriteRecipeDto = _mapper.Map<IEnumerable<FavoriteRecipeDto>>(dbFavoriteRecipe);
        return Ok(favoriteRecipeDto);
    }
    
    [HttpDelete("{favoriteRecipeId}"), Authorize]
    public async Task<IActionResult> DeleteApplicationUserFavoriteRecipe(int favoriteRecipeId)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var applicationUserFavoriteRecipe = await _repository.FavoriteRecipe.GetApplicationUserFavoriteRecipe(user.Id, favoriteRecipeId, trackChanges: false);
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