using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    IEnumerable<Step> GetRecipeSteps(int recipeId, bool trackChanges);

    Step GetStep(int stepId, bool trackChanges);
    void CreateRecipeStep(int recipeId, Step step);
}