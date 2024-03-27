using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.Models;

public class Step
{
    [Column("StepId")]
    public int StepId { get; set; }
    [Required(ErrorMessage = "Step description is required.")]
    public string Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Поле 'Order' не должно быть пустым")]
    public int Order { get; set; }
    public int RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}