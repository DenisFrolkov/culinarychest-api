using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipesAsync(RecipeParameters recipeParameters, bool trackChanges);
    Task<List<Recipe>> GetApplicationUserRecipesAsync(int authorId, ApplicationUserRecipeParameters applicationUserRecipeParameters, bool trackChanges);
    Task<Recipe> GetApplicationUserRecipeAsync(int authorId, int recipeId, bool trackChanges);
    Task<Recipe> GetRecipeAsync(int recipeId, bool trackChanges);
    void CreateApplicationUserRecipe(int authorId, Recipe recipe);
    void DeleteRecipe(Recipe recipe);
}