using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

public class CommentViewModel
{
    [Required]
    public int AdvertismentId { get; set; }
    [Required]
    [MaxLength(300)]
    public required string Content { get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    [BindNever]
    public string? OwnerName { get; set; }
}