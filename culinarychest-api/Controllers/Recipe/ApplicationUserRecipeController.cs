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

[Route("api/applicationUser/recipe")]
[ApiController]
public class ApplicationUserRecipeController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;


    public ApplicationUserRecipeController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Вывод всех рецептов созданных пользователем
    /// </summary>
    /// <returns> Список рецептов созданных пользователем</returns>.
    [HttpGet, Authorize]
    public async Task<IActionResult> GetApplicationUserRecipes(
        [FromQuery] ApplicationUserRecipeParameters applicationUserRecipeParameters)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var dbRecipes =
            await _repository.Recipe.GetApplicationUserRecipesAsync(user.Id, applicationUserRecipeParameters,
                trackChanges: false);
        foreach (var recipe in dbRecipes)
        {
            recipe.Steps = await _repository.Step.GetRecipeSteps(recipe.RecipeId, trackChanges: false);
        }

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dbRecipes.MetaData));
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(dbRecipes);
        return Ok(recipesDto);
    }

    /// <summary>
    /// Создание рецептов пользователем
    /// </summary>
    /// <returns> Успешное создание рецепта</returns>.
[HttpPost, Authorize]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public async Task<IActionResult> CreateApplicationUserRecipe([FromForm] CreateRecipeDto recipe)
{
    if (recipe == null)
    {
        return BadRequest("Recipe data is null.");
    }

    var userName = User.FindFirstValue(ClaimTypes.Name);
    if (string.IsNullOrEmpty(userName))
    {
        return Unauthorized("User not found.");
    }

    var user = await _userManager.FindByNameAsync(userName);
    if (user == null)
    {
        return Unauthorized("User not found.");
    }

    ICollection<Step> steps;
    try
    {
        steps = JsonConvert.DeserializeObject<ICollection<Step>>(recipe.steps);
    }
    catch (Exception ex)
    {
        return BadRequest("Invalid steps format.");
    }

    var recipeEntity = new Recipe
    {
        Title = recipe.title,
        Ingredients = recipe.ingredients,
        CreationDate = recipe.creationDate,
        PreparationTime = recipe.preparationTime,
        Steps = steps,
        Id = user.Id
    };

    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    if (!Directory.Exists(imagesPath))
    {
        Directory.CreateDirectory(imagesPath);
    }

    if (recipe.RecipeImage != null)
    {
        var imageName = $"{Guid.NewGuid()}_{recipe.RecipeImage.FileName}.png";
        var imagePath = Path.Combine(imagesPath, imageName);
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await recipe.RecipeImage.CopyToAsync(stream);
        }

        recipeEntity.RecipeImage = imageName; // Сохраняем только имя файла
    }

    _repository.Recipe.CreateApplicationUserRecipe(user.Id, recipeEntity);
    await _repository.SaveAsync();

    var successMessage = "Recipe has been created successfully.";
    var recipeToReturn = _mapper.Map<RecipeDto>(recipeEntity);
    return CreatedAtRoute(new
    {
        authorId = user.Id,
        id = recipeToReturn.RecipeId
    }, successMessage);
}
[]



    /// <summary>
    /// Удаление рецепта пользователем
    /// </summary>
    /// <returns> Успешное удаление рецепта</returns>.
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

    /// <summary>
    /// Изменение рецепта пользователем
    /// </summary>
    /// <returns> Успешное изменение рецепта</returns>.
    [HttpPut("{recipeId}"), Authorize]
    public async Task<IActionResult> UpdateApplicationUserRecipe(int recipeId, [FromForm] UpdateRecipeDto recipe)
    {
        var recipeEntity = await _repository.Recipe.GetRecipe(recipeId, trackChanges: true);
        if (recipeEntity == null)
        {
            _logger.LogInfo($"Recipe with id: {recipeId} doesn't exist in the database.");
            return NotFound();
        }

        recipeEntity.Title = recipe.Title;
        recipeEntity.Ingredients = recipe.Ingredients;
        recipeEntity.CreationDate = recipe.CreationDate;
        recipeEntity.PreparationTime = recipe.PreparationTime;

        if (recipe.RecipeImage != null)
        {
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileExtension = Path.GetExtension(recipe.RecipeImage.FileName);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await recipe.RecipeImage.CopyToAsync(fileStream);
            }

            recipeEntity.RecipeImage = $"/images/{uniqueFileName}";
        }

        await _repository.SaveAsync();

        return NoContent();
    }
}