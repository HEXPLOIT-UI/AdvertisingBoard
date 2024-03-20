using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Advertisement
{
    [Key]
    public int AdvertisementId { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    [Required]
    public User User { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [Required]
    public Category Category { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public double Price { get; set; }
    public string? ContactInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Photo> Photos { get; set; } = [];
}
