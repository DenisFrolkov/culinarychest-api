using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class ManipulationFavoriteRecipeDto
{
    [Required(ErrorMessage = "Data is required.")]
    public DateTime AddedDate { get; set; }
}