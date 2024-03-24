using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    IEnumerable<Step> GetRecipeForSteps(int recipeId, bool trackChanges);
    void CreateRecipeStep(int recipeId, Step step);
}