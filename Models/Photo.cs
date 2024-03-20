using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Photo
{
    [Key]
    public int PhotoId { get; set; }
    public int AdvertisementId { get; set; }
    [ForeignKey("AdvertisementId")]
    [Required]
    public Advertisement Advertisement { get; set; }
    public string? PhotoURL { get; set; }
    public string? Description { get; set; }
}