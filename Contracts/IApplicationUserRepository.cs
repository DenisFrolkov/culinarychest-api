using Entities.Models;

namespace Contracts;

public interface IApplicationUserRepository
{
    IEnumerable<ApplicationUser> GetAllApplicationUsers(bool trackChanges);
    ApplicationUser GetApplicationUser(int userId, bool trackChanges);
    void CreateApplicationUser(ApplicationUser applicationUser);
}