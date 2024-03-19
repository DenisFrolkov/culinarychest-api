using Entities.Models;

namespace Entities.DataTransferObjects;

public class RecipeDto
{
    public int RecipeId { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public byte[] RecipeImage { get; set; }
    public string Ingredients { get; set; }
    public ICollection<Step> Steps { get; set; }
    public DateTime CreationDate { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public int SavedCount { get; set; }    
}