using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<FavoriteRecipe> GetAllFavoriteRecipes(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.FavoriteRecipeId)
            .ToList();

    public FavoriteRecipe GetFavoriteRecipe(int FavoriteRecipeId, bool trackChanges) =>
        FindByCondition(favoriteRecipe => 
                favoriteRecipe.FavoriteRecipeId.Equals(FavoriteRecipeId), trackChanges).SingleOrDefault();
}