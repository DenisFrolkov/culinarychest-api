using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
    
    public IEnumerable<FavoriteRecipe> GetApplicationUserFavoriteRecipes(int authorId, bool trackChanges) =>
        FindByCondition(favoriteRecipe => favoriteRecipe.AuthorId.Equals(authorId), trackChanges)
            .OrderBy(e => e.RecipeId);

    public FavoriteRecipe GetApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId, bool trackChanges) =>
        FindByCondition(favoriteRecipe => 
                favoriteRecipe.AuthorId.Equals(authorId) && favoriteRecipe.FavoriteRecipeId.Equals(favoriteRecipeId), trackChanges)
            .SingleOrDefault();

    public void CreateApplicationUserFavoriteRecipe(int authorId, int recipeId, FavoriteRecipe favoriteRecipe)
    {
        favoriteRecipe.AuthorId = authorId;
        favoriteRecipe.RecipeId = recipeId;
        Create(favoriteRecipe);
    }

    public void DeleteFavoriteRecipe(FavoriteRecipe favoriteRecipe)
    {
        Delete(favoriteRecipe);
    }
}