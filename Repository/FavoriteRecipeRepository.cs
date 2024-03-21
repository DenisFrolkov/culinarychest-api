using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class FavoriteRecipeRepository : RepositoryBase<FavoriteRecipe>, IFavoriteRecipeRepository
{
    public FavoriteRecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
    
    public IEnumerable<FavoriteRecipe> GetAllFavoriteRecipeForApplicationUser(int authorId, bool trackChanges) =>
        FindByCondition(favoriteRecipe => favoriteRecipe.AuthorId.Equals(authorId), trackChanges)
            .OrderBy(e => e.RecipeId);

    public void CreateFavoriteRecipe(FavoriteRecipe favoriteRecipe) => Create(favoriteRecipe);
}