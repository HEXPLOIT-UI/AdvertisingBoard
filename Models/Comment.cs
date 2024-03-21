
using System.ComponentModel.DataAnnotations;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    [Required]
    public int AdvertismentId {  get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    [MaxLength(300)]
    public string Content {  get; set; }
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}