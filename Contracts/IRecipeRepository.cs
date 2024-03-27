using Entities.Models;

namespace Contracts;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetRecipes(bool trackChanges);
    Task<List<Recipe>> GetApplicationUserRecipes(int authorId, bool trackChanges);
    Task<Recipe> GetApplicationUserRecipe(int authorId, int recipeId, bool trackChanges);
    Task<Recipe> GetRecipe(int recipeId, bool trackChanges);
    void CreateApplicationUserRecipe(int authorId, Recipe recipe);
    void DeleteRecipe(Recipe recipe);
}