using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Recipe> CreatedRecipes { get; set; }
    public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }
}