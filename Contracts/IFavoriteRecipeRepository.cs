using Entities.Models;

namespace Contracts;

public interface IFavoriteRecipeRepository
{
    IEnumerable<FavoriteRecipe> GetAllFavoriteRecipeForApplicationUser(int authorId, bool trackChanges);
}
