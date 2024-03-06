using System.ComponentModel.DataAnnotations;

public class CustomPasswordValidationAttribute : ValidationAttribute
{
    private readonly int _minLength;
    private readonly int _maxLength;
    private readonly bool _requireDigit;
    private readonly bool _requireLowercase;
    private readonly bool _requireUppercase;

    public CustomPasswordValidationAttribute(int minLength, int maxLength,
                                             bool requireDigit = true,
                                             bool requireLowercase = true,
                                             bool requireUppercase = true)
    {
        _minLength = minLength;
        _maxLength = maxLength;
        _requireDigit = requireDigit;
        _requireLowercase = requireLowercase;
        _requireUppercase = requireUppercase;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = (string)value;

        if (string.IsNullOrEmpty(password))
        {
            return new ValidationResult("Password is required");
        }

        if (password.Length < _minLength || password.Length > _maxLength)
        {
            return new ValidationResult($"Password must be between {_minLength} and {_maxLength} characters long");
        }

        if (_requireDigit && !ContainsDigit(password))
        {
            return new ValidationResult("Password must contain at least one digit");
        }

        if (_requireLowercase && !ContainsLowercase(password))
        {
            return new ValidationResult("Password must contain at least one lowercase letter");
        }

        if (_requireUppercase && !ContainsUppercase(password))
        {
            return new ValidationResult("Password must contain at least one uppercase letter");
        }

        return ValidationResult.Success;
    }

    private bool ContainsDigit(string password)
    {
        foreach (char c in password)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainsLowercase(string password)
    {
        foreach (char c in password)
        {
            if (char.IsLower(c))
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainsUppercase(string password)
    {
        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                return true;
            }
        }
        return false;
    }
}
