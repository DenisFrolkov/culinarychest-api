using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class RepositoryContext : DbContext
{
    //Этот конструктор принимает параметр DbContextOptions, который содержит настройки для
    //подключения к базе данных и других опций контекста. Эти настройки передаются в базовый
    //класс DbContext через вызов base(options).
    public RepositoryContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteRecipeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        modelBuilder.ApplyConfiguration(new StepConfiguration());
    }

    //Это свойства, которые представляют собой наборы сущностей, соответствующих таблицам в базе данных.
    //Каждый DbSet позволяет выполнять операции CRUD (создание, чтение, обновление, удаление) для соответствующих сущностей
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
}