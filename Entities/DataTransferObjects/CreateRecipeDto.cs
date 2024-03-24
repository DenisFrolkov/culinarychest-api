using Entities.Models;

namespace Entities.DataTransferObjects;

public class CreateRecipeDto
{
    public string Title { get; set; }
    public byte[] RecipeImage { get; set; }
    public string Ingredients { get; set; }
    public IEnumerable<CreateStepsDto> Steps { get; set; }
    public DateTime CreationDate { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public int SavedCount { get; set; }    
}