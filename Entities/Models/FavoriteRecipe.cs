using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class FavoriteRecipe
{
    [Column("FavoriteRecipeId")]
    public int FavoriteRecipeId { get; set; }
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "Data is required.")]
    public DateTime AddedDate { get; set; }
    [ForeignKey("AuthorId")]
    public ApplicationUser Author { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}