using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IRecipeRepository
{
    Task<PagedList<Recipe>> GetRecipesAsync(RecipeParameters recipeParameters, bool trackChanges);
    Task<PagedList<Recipe>> GetRecipeByIdsAsync(List<int> recipeIds, RecipeParameters recipeParameters, bool trackChanges);
    Task<PagedList<Recipe>> GetApplicationUserRecipesAsync(string authorId,
        ApplicationUserRecipeParameters applicationUserRecipeParameters, bool trackChanges);
    Task<Recipe> GetApplicationUserRecipeAsync(string authorId, int recipeId, bool trackChanges);
    Task<PagedList<Recipe>> GetRecipeByIdsAsync(int recipeId, RecipeParameters recipeParameters, bool trackChanges);
    Task<Recipe> GetRecipe(int recipeId, bool trackChanges);
    void CreateApplicationUserRecipe(string authorId, Recipe recipe);
    void DeleteRecipe(Recipe recipe);
}