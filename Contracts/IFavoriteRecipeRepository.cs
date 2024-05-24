using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    Task<PagedList<FavoriteRecipe>> GetApplicationUserFavoriteRecipes(string authorId, ApplicationUserFavoriteRecipeParameters applicationUserFavoriteRecipeParameters, bool trackChanges);
    Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(string authorId, int recipeId, bool trackChanges);
    Task<FavoriteRecipe> GetFavoriteRecipeByRecipeId(string authorId, int recipeId, bool trackChanges);
    void CreateApplicationUserFavoriteRecipe(string authorId, int recipeId, FavoriteRecipe favoriteRecipe);
    
    void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe);
}
