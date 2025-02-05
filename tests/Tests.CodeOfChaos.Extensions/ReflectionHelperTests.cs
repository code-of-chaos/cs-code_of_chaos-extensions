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
            int valueType,
            int? nullableValueType
        ) { }
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
    
    [Test]
    public async Task IsNullableReferenceType_ShouldReturnFalse_ForNullableValueType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo nullableValueTypeParameter = method.GetParameters().First(p => p.Name == "nullableValueType");

        // Act
        bool result = nullableValueTypeParameter.IsNullableReferenceType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'nullableInt' to be recognized as not a nullable reference type (it's a nullable value type).");
    }

    [Test]
    public async Task IsNullableValueType_ShouldReturnFalse_ForReferenceType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo referenceTypeParameter = method.GetParameters().First(p => p.Name == "nonNullable");

        // Act
        bool result = referenceTypeParameter.IsNullableValueType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'nonNullable' to not be a nullable value type (it's a reference type).");
    }

    [Test]
    public async Task IsNullableValueType_ShouldReturnFalse_ForNonNullableValueType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo valueTypeParameter = method.GetParameters().First(p => p.Name == "valueType");

        // Act
        bool result = valueTypeParameter.IsNullableValueType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'valueType' to not be a nullable value type (it's a non-nullable value type).");
    }

    [Test]
    public async Task IsNullableValueType_ShouldReturnTrue_ForNullableValueType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo nullableValueTypeParameter = method.GetParameters().First(p => p.Name == "nullableValueType");

        // Act
        bool result = nullableValueTypeParameter.IsNullableValueType();

        // Assert
        await Assert.That(result).IsTrue().Because("Expected parameter 'nullableValueType' to be recognized as a nullable value type.");
    }

    [Test]
    public async Task IsNullableValueType_ShouldReturnFalse_ForNullableReferenceType()
    {
        // Arrange
        MethodInfo method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithNullability))!;
        ParameterInfo nullableParameter = method.GetParameters().First(p => p.Name == "nullable");

        // Act
        bool result = nullableParameter.IsNullableValueType();

        // Assert
        await Assert.That(result).IsFalse().Because("Expected parameter 'nullable' to not be a nullable value type (it's a nullable reference type).");
    }

}
