using System.ComponentModel.DataAnnotations;

public class RegisterUserViewModel
{
    [Required]
    [StringLength(128, MinimumLength = 3)]
    [RegularExpression(@"^\p{L}+\s+\p{L}+$", ErrorMessage = "Полное имя должно быть в формате 'Имя Фамилия'")]
    public required string FullName { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только латинские буквы и цифры без пробелов")]
    public required string Login { get; set; }
    [Required]
    [StringLength(128, MinimumLength = 3)]
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
    public required string Email { get; set; }
    [Required]
    [StringLength(128, MinimumLength = 6)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Пароль может содержать только латинские буквы и цифры")]
    public required string Password { get; set; }
    [Required]
    [StringLength(64, MinimumLength = 3)]
    [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Неверный формат номера телефона")]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public required string PhoneNumber { get; set; }
}