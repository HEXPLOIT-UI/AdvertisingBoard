public class CategoryViewModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public CategoryViewModel? ParentCategory { get; set; }
    public ICollection<CategoryViewModel> SubCategories { get; set; } = [];
}