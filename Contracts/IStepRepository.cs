using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    IEnumerable<Step> GetRecipeForSteps(int recipeId, bool trackChanges);
    void CreateStep(Step step);
}