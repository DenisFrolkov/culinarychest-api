using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteRecipeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        modelBuilder.ApplyConfiguration(new StepConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
}