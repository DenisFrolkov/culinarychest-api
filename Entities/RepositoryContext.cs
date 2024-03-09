using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options){}
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
}