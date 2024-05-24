using Entities.Models;

namespace Entities.DataTransferObjects;

public class RecipeDto
{
    public int RecipeId { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    // public string RecipeImage { get; set; }
    public string Ingredients { get; set; }
    public ICollection<StepDto> Steps { get; set; }
    public string CreationDate { get; set; }
    public string PreparationTime { get; set; }
    public int SavedCount { get; set; }    
}