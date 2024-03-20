using System.ComponentModel.DataAnnotations;

public class PhotoViewModel
{
    [Required]
    public required string PhotoURL { get; set; }
    public string? Description { get; set; }
}