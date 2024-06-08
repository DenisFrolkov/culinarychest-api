using System.ComponentModel.DataAnnotations;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Entities.DataTransferObjects;

public class CreateRecipeDto
{
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string title { get; set; }
    [Required(ErrorMessage = "Recipe images - required field.")]
    public IFormFile RecipeImage { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public string ingredients { get; set; }
    [Required(ErrorMessage = "A recipe must have at least one field with a recipe pitch.")]
    public string steps { get; set; }
    [Required(ErrorMessage = "Recipe creationDate - required field.")]
    public string creationDate { get; set; }
    [Required(ErrorMessage = "Recipe preparationTime - required field.")]
    public string preparationTime { get; set; }
}