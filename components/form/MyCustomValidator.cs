using System;
using System.ComponentModel.DataAnnotations;

public class MyCustomValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {
        Console.WriteLine($"MyCustomValidator value:{value}");
        return new ValidationResult("Validation message to user.",
            new[] { validationContext.MemberName });
    }
}
