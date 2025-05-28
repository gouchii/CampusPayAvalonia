using System;
using System.ComponentModel.DataAnnotations;

namespace ClientApp.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NumAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Value cannot be null.");
        }

        if (value is decimal decimalValue)
        {
            if (decimalValue <= 0)
            {
                return new ValidationResult("Amount must be a positive number.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid number format.");
    }
}