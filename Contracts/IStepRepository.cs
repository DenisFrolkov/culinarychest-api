using Entities.Models;

namespace Contracts;

public interface IStepRepository
{
    Task<List<Step>> GetRecipeSteps(int recipeId, bool trackChanges);
    Task<Step> GetStep(int stepId, bool trackChanges);
    void CreateRecipeStep(int recipeId, Step step);
}