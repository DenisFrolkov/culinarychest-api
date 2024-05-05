using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Recipe
{
    [Key]
    [Column("RecipeId")]
    public int RecipeId { get; set; }
    
    [ForeignKey("Author")]
    public string Id { get; set; }
    
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Recipe images - required field.")]
    public string RecipeImage { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "A recipe must have at least one field with a recipe pitch.")]
    public ICollection<Step> Steps { get; set; }
    public string CreationDate { get; set; }
    public string PreparationTime { get; set; }
    public int SavedCount { get; set; }    
}

