using System.ComponentModel.DataAnnotations;

namespace AdvertisingBoard.ModelsDTO
{
    public class LoginUserViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только латинские буквы и цифры без пробелов")]
        public required string Login;
        [Required]
        [StringLength(128, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Пароль может содержать только латинские буквы и цифры")]
        public required string Password;
    }
}
