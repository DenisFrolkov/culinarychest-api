using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class StepRepository : RepositoryBase<Step>, IStepRepository
{
    public StepRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<Step> GetAllSteps(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.RecipeId)
            .ToList();
}