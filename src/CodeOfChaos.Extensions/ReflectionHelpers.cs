// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ReflectionHelper {
    public static bool IsNullableReferenceType(this ParameterInfo parameter) {
        // Value types can't be nullable reference types
        if (parameter.ParameterType.IsValueType)
            return false;

        // Check if NullableAttribute exists on this parameter
        CustomAttributeData? nullableAttribute = parameter.GetCustomAttributesData()
            .FirstOrDefault(attr => attr.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

        // If there is no NullableAttribute, treat it as non-nullable
        if (nullableAttribute == null || nullableAttribute.ConstructorArguments.Count == 0)
            return false;

        // NullableAttribute encodes nullability flags; we need the first value for this parameter
        byte? nullabilityFlag = nullableAttribute.ConstructorArguments[0].Value as byte? ;

        // "2" = Nullable; "0" = Non-nullable; "1" = Oblivious (treat as non-nullable in this context)
        return nullabilityFlag == 2;

    }
}
