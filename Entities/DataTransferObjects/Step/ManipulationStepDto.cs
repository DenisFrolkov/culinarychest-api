using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class ManipulationStepDto
{
    [Required(ErrorMessage = "Step description is required.")]
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Поле 'Order' не должно быть пустым")]
    public int Order { get; set; }
}