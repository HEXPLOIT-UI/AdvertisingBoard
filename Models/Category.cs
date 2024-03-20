using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    [ForeignKey("ParentCategoryId")]
    public Category ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = [];
}
