using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.DataTransferObjects;

public class CreateRecipeDto
{
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Recipe images - required field.")]
    public string RecipeImage { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "A recipe must have at least one field with a recipe pitch.")]
    public IEnumerable<CreateStepsDto> Steps { get; set; }
    [Required(ErrorMessage = "Recipe creationDate - required field.")]
    public string CreationDate { get; set; }
    [Required(ErrorMessage = "Recipe preparationTime - required field.")]
    public string PreparationTime { get; set; }
}