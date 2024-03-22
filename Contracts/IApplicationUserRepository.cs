using Entities.Models;

namespace Contracts;

public interface IApplicationUserRepository
{
    ApplicationUser GetApplicationUser(int userId, bool trackChanges);
    void CreateApplicationUser(ApplicationUser applicationUser);
}