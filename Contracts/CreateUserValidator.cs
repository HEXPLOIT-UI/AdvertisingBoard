using AdvertisingBoard.Data;
using System.ComponentModel.DataAnnotations;

public class CreateUserValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            var currentValue = (string)value;
            var existingEntity = dbContext?.Users.FirstOrDefault(e => e.Login == currentValue || e.Email == currentValue || e.PhoneNumber == currentValue);
            if (existingEntity != null)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        return ValidationResult.Success;
    }
}