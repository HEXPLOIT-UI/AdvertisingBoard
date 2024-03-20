using System.ComponentModel.DataAnnotations;

public class CategoryViewModel
{
    [Required]
    public required string Name { get; set; }
    public int? ParentCategoryId { get; set; }
}