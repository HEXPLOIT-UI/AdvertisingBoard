using System.ComponentModel.DataAnnotations;

public class Photo
{
    [Key]
    public int PhotoId { get; set; }
    public int AdvertisementId { get; set; }
    public string PhotoURL { get; set; }
    public int UserId { get; set; }
}