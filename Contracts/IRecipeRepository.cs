using Entities.Models;

namespace Contracts;

public interface IRecipeRepository
{
    IEnumerable<Recipe> GetRecipes(bool trackChanges);
    IEnumerable<Recipe> GetApplicationUserRecipes(int authorId ,bool trackChanges);
    Recipe GetRecipe(int recipeId, bool trackChanges);
    void CreateApplicationUserRecipe(int authorId, Recipe recipe);

}