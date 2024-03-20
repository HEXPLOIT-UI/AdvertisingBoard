using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId {  get; set; }
}