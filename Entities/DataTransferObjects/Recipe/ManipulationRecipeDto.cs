using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract class ManipulationRecipeDto
{
    [Required(ErrorMessage = "Recipe title - required field.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Recipe images - required field.")]
    public byte[] RecipeImage { get; set; }
    [Required(ErrorMessage = "Recipe ingredients - required field.")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "A recipe must have at least one field with a recipe pitch.")]
    public IEnumerable<CreateStepsDto> Steps { get; set; }
    [Required(ErrorMessage = "Recipe creationDate - required field.")]
    public DateTime CreationDate { get; set; }
    [Required(ErrorMessage = "Recipe preparationTime - required field.")]
    public TimeSpan PreparationTime { get; set; }
}