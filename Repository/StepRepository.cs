using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class StepRepository : RepositoryBase<Step>, IStepRepository
{
    public StepRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}