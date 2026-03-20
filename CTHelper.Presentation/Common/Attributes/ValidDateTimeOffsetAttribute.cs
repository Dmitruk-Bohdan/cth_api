using System.ComponentModel.DataAnnotations;

namespace CTHelper.Presentation.Common.Attributes;

public class ValidDateTimeOffsetAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        if (value is not string str)
            return new ValidationResult("Invalid type. Expected string.");

        if (DateTimeOffset.TryParse(str, out _))
            return ValidationResult.Success;

        return new ValidationResult("Invalid DateTimeOffset format.");
    }
}