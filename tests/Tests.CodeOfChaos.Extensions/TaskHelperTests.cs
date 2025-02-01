// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;

namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TaskHelperTests {
    [Test]
    [Arguments(null, null)]
    [Arguments(1, 1)]
    public async Task FromTaskOrDefault_ShouldReturnDefault_int(int? input, int? expected) {
        // Arrange
        Task<int?> task = Task.FromResult(input);
        
        // Act
        Task<int?> result = TaskHelper.FromTaskOrDefault(task);
        
        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
    
    [Test]
    [Arguments(true, true)]
    [Arguments(false, false)]
    public async Task FromTaskOrDefault_ShouldReturnExpected_ReferenceType(bool setAsDefault, bool expectedIsNull) {
        // Arrange
        var expected = new TestClass();
        Task<TestClass?> task = Task.FromResult(setAsDefault ? null : new TestClass());
        
        // Act
        Task<TestClass?> result = TaskHelper.FromTaskOrDefault(task);
        
        // Assert
        await Assert.That(await result is null).IsEqualTo(expectedIsNull);
    }
    
    private class TestClass {}
}
