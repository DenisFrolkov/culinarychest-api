using Entities.DataTransferObjects;

namespace Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(AuthenticationApplicationUserDto authenticationApplicationUserForAuth);
    Task<string> CreateToken(); 
}