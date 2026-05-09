using System.ComponentModel.DataAnnotations;

namespace CoreLib.Validation.Attributes;

/// <summary>
/// Проверяет, что строка входит в список допустимых значений.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class AllowedValuesAttribute : ValidationAttribute
{
    private readonly HashSet<string> _allowedValues;

    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = new HashSet<string>(allowedValues, StringComparer.OrdinalIgnoreCase);
        ErrorMessage = $"Допустимые значения: {string.Join(", ", allowedValues)}.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (value is not string stringValue)
            return false;

        return _allowedValues.Contains(stringValue);
    }
}