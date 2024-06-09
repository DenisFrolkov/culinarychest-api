using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Entities.DataTransferObjects;

public class UpdateRecipeDto
{
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string Title { get; set; }
    public IFormFile RecipeImage { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "Recipe creationDate - required field.")]
    public string CreationDate { get; set; }
    [Required(ErrorMessage = "Recipe preparationTime - required field.")]
    public string PreparationTime { get; set; }
}