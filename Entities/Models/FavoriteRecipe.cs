using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class FavoriteRecipe
{
    [Column("FavoriteRecipeId")]
    public int FavoriteRecipeId { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int RecipeId { get; set; }
    public DateTime AddedDate { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}