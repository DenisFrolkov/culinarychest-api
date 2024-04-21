using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class FavoriteRecipe
{
    [Key]
    [Column("FavoriteRecipeId")]
    public int FavoriteRecipeId { get; set; }
    [ForeignKey("Author")]
    public string Id { get; set; }
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "Data is required.")]
    public DateTime AddedDate { get; set; }
    
    [ForeignKey("Id")]
    public User Author { get; set; }
    
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}