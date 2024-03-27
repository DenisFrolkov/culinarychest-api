using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class StepRepository : RepositoryBase<Step>, IStepRepository
{
    public StepRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public Task<List<Step>> GetAllSteps(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.RecipeId)
            .ToListAsync();

    public async Task<Step> GetStep(int stepId, bool trackChanges) =>
        await FindByCondition(step => 
            step.StepId.Equals(stepId), trackChanges).SingleOrDefaultAsync();
    
    public async Task<List<Step>> GetRecipeSteps(int recipeId, bool trackChanges) =>
        await FindByCondition(step => step.RecipeId.Equals(recipeId), trackChanges)
            .OrderBy(e => e.StepId).ToListAsync();

    public void CreateRecipeStep(int recipeId, Step step)
    {
        step.RecipeId = recipeId;
        Create(step);
    }
}