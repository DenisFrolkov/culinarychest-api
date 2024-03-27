using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract class ManipulationApplicationUserDto
{
    [Required(ErrorMessage = "Login is required.")]
    [MaxLength(20, ErrorMessage = "The maximum length of the login is 50 characters.")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(99, ErrorMessage = "The maximum length of the email is 99 characters.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(16, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }
}