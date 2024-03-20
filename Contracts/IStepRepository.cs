using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    IEnumerable<Step> GetRecipeForAllSteps(int recipeId, bool trackChanges);
}