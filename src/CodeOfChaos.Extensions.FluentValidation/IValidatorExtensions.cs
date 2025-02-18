// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;

namespace CodeOfChaos.Extensions.FluentValidation;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ValidatorExtensions {
    public static bool TryValidate<T>(this IValidator<T> validator, T instance, out List<ValidationFailure> failures) {
        ValidationResult result = validator.Validate(instance);
        failures = result.Errors;
        return result.IsValid;
    }
    
    public static void ThrowIfInvalid<T>(this IValidator<T> validator, T instance) {
        ValidationResult result = validator.Validate(instance);
        if (!result.IsValid) throw new ValidationException(result.Errors);
    }
    
    public static IEnumerable<string> ValidateAndGetErrorMessages<T>(this IValidator<T> validator, T instance) {
        ValidationResult result = validator.Validate(instance);
        return result.Errors.Select(e => e.ErrorMessage);
    }

    public static List<ValidationFailure> ValidateAndGetErrors<T>(this IValidator<T> validator, T instance) {
        ValidationResult result = validator.Validate(instance);
        return result.Errors;
    }
    
    public static T ValidateOrDefault<T>(this IValidator<T> validator, T instance, Func<T> defaultValueFactory) {
        ValidationResult result = validator.Validate(instance);
        return result.IsValid ? instance : defaultValueFactory();
    }

    public static async ValueTask<T?> ValidateOrDefaultAsync<T>(this IValidator<T> validator, T instance, Func<ValueTask<T>>? defaultValueFactory = null)  {
        ValidationResult result = await validator.ValidateAsync(instance);
        if (result.IsValid) return instance;
        if (defaultValueFactory is null) return default;
        return await defaultValueFactory();
    }
}
