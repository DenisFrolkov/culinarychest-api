namespace Entities.DataTransferObjects;

public class FavoriteRecipeDto
{
    public int FavoriteRecipeId { get; set; }
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public DateTime AddedDate { get; set; }
}