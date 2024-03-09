using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Recipe
{
    [Column("RecipeId")]
    public int Id { get; set; }
    public string AuthorId { get; set; }
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public byte[] RecipeImage { get; set; }
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "A recipe must have at least one field with a recipe pitch.")]
    public ICollection<Step> Steps { get; set; }
    public DateTime CreationDate { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public int SavedCount { get; set; }    
    [ForeignKey("AuthorId")]
    public ApplicationUser Author { get; set; }
}

