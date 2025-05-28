using System;
using System.ComponentModel.DataAnnotations;

namespace ClientApp.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NumAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}

