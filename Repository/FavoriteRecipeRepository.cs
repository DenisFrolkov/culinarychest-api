using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}