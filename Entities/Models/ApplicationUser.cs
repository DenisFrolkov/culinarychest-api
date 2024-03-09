using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class ApplicationUser
{
    [Column("ApplicationUserId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Login is required.")]
    [MaxLength(20, ErrorMessage = "The maximum length of the login is 50 characters.")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(99, ErrorMessage = "The maximum length of the email is 255 characters.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(16, ErrorMessage = "Password must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public ICollection<Recipe> CreatedRecipes { get; set; }
    public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }
}