using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract class ManipulationFavoriteRecipeDto
{
    [Required(ErrorMessage = "Data is required.")]
    public String AddedDate { get; set; }
}