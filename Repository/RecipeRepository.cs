using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}