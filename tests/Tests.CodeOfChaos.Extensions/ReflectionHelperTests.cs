// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using System.Reflection;

namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ReflectionHelperTests {
    private class TestClass
    {
        // ReSharper disable UnusedParameter.Local
        public static void MethodWithNullability(
            string nonNullable, 
            string? nullable, 
            int valueType) 
        { }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task IsNullableReferenceType_ShouldReturnTrue_ForNullableReferenceType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo nullableParameter = method.GetParameters().First(p => p.Name == "nullable");

        // Act
        bool result = nullableParameter.IsNullableReferenceType();

        // Assert
        await Assert.That(result).IsTrue().Because("Expected parameter 'nullable' to be recognized as a nullable reference type.");
    }

    [Test]
    public async Task IsNullableReferenceType_ShouldReturnFalse_ForNonNullableReferenceType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo nonNullableParameter = method.GetParameters().First(p => p.Name == "nonNullable");

        // Act
        bool result = nonNullableParameter.IsNullableReferenceType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'nonNullable' to be recognized as a non-nullable reference type.");
    }

    [Test]
    public async Task IsNullableReferenceType_ShouldReturnFalse_ForValueType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo valueTypeParameter = method.GetParameters().First(p => p.Name == "valueType");

        // Act
        bool result = valueTypeParameter.IsNullableReferenceType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'valueType' to be recognized as a non-nullable reference type.");
    }

}
