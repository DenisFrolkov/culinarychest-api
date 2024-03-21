namespace Entities.DataTransferObjects;

public class FavoriteRecipeForCreationDto
{
    public int AuthorId { get; set; }
    public int RecipeId { get; set; }
    public DateTime AddedDate { get; set; }
}