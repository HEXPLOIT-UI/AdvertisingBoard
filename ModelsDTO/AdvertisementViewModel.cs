using System.ComponentModel.DataAnnotations;

public class AdvertisementViewModel
{
    [Required]
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public string ContactInfo { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
}