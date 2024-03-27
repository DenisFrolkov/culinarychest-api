using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
    
    public async Task<List<FavoriteRecipe>> GetApplicationUserFavoriteRecipes(int authorId, bool trackChanges) =>
        await FindByCondition(favoriteRecipe => favoriteRecipe.AuthorId.Equals(authorId), trackChanges)
            .OrderBy(e => e.RecipeId)
            .ToListAsync();

    public async Task<FavoriteRecipe> GetApplicationUserFavoriteRecipe(int authorId, int favoriteRecipeId, bool trackChanges) =>
        await FindByCondition(favoriteRecipe => 
                favoriteRecipe.AuthorId.Equals(authorId) && favoriteRecipe.FavoriteRecipeId.Equals(favoriteRecipeId), trackChanges)
            .SingleOrDefaultAsync();

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