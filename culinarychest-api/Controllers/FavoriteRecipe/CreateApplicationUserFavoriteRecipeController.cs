using System.Security.Claims;
using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/favoriteRecipe/{recipeId}")]
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
    
    /// <summary>
    /// Добавить рецепт в сохранненые пользователем
    /// </summary>
    /// <returns> Успешное сохранение рецепта в сохраненные пользователем</returns>.
    [HttpPost, Authorize]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUserFavoriteRecipe(int recipeId, CreateFavoriteRecipeDto favoriteRecipe)
    {
        var recipe = await _repository.Recipe.GetRecipe(recipeId, trackChanges: false);
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
        
        var successMessage = "You successfully saved the recipe.";
        
        var favoriteRecipeToReturn = _mapper.Map<FavoriteRecipeDto>(favoriteRecipeEntity);
        return CreatedAtRoute(new { UserId = user.Id, Id = favoriteRecipeToReturn.FavoriteRecipeId }, successMessage);
    }
    
    /// <summary>
    /// Удалить рецепта из сохраненных пользователем
    /// </summary>
    /// <returns> Успешное удаление рецепта из сохранненых</returns>.
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteApplicationUserFavoriteRecipe(int recipeId)
    {
        try
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(userName);
            
            var dbFavoriteRecipe = await _repository.FavoriteRecipe.GetFavoriteRecipeByRecipeId(user.Id, recipeId, trackChanges: false);
            var favoriteRecipeDto = _mapper.Map<FavoriteRecipeDto>(dbFavoriteRecipe);
            
            var applicationUserFavoriteRecipe = await _repository.FavoriteRecipe.GetApplicationUserFavoriteRecipe(user.Id, favoriteRecipeDto.FavoriteRecipeId, trackChanges: false);
            if (applicationUserFavoriteRecipe == null)
            {
                _logger.LogInfo($"FavoriteRecipe with id: {recipeId} doesn't exist in the database.");
                return NotFound();
            }
            _repository.FavoriteRecipe.DeleteFavoriteRecipe(applicationUserFavoriteRecipe);
            await _repository.SaveAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting favorite recipe with id {recipeId}: {ex}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetFavoriteRecipeByRecipeId(int recipeId)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        
        var dbFavoriteRecipe = await _repository.FavoriteRecipe.GetFavoriteRecipeByRecipeId(user.Id, recipeId, trackChanges: false);
        
        if (dbFavoriteRecipe == null)
        {
            _logger.LogInfo($"FR with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }
        var favoriteRecipeDto = _mapper.Map<FavoriteRecipeDto>(dbFavoriteRecipe);
        return Ok(favoriteRecipeDto);
    }
}