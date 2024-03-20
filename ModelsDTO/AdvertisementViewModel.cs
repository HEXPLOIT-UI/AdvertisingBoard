using System.ComponentModel.DataAnnotations;

public class AdvertisementViewModel
{
    [Required]
    public required string Title { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? ContactInfo { get; set; }
    [Required]
    public required string CategoryName { get; set; }
    public ICollection<PhotoViewModel> Photos { get; set; } = [];
}