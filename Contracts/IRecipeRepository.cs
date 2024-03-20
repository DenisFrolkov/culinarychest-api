using Entities.Models;

namespace Contracts;

public interface IRecipeRepository
{
    IEnumerable<Recipe> GetAllRecipes(bool trackChanges);
    IEnumerable<Recipe> GetApplicationUserAllRecipes(int authorId ,bool trackChanges);
    Recipe GetRecipe(int recipeId, bool trackChanges);

}