using Entities.Models;

namespace Contracts;

public interface IApplicationUserRepository
{
    Task<ApplicationUser> GetApplicationUser(string userId, bool trackChanges);
    void CreateApplicationUser(ApplicationUser applicationUser);
    void DeleteApplicationUser(ApplicationUser applicationUser);
}