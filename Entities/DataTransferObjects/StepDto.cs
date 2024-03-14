namespace Entities.DataTransferObjects;

public class StepDto
{
    public int StepId { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public int RecipeId { get; set; }
}