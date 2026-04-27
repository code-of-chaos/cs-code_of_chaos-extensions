// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.ObjectPool;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaosTests.Extensions.ObjectPool;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ResettablePoolPolicyTests {
    public class TestResettable : IResettable {
        public bool ResetSuccessful { get; set; } = true;
        public int ResetCallCount { get; private set; }

        public bool TryReset() {
            if (!ResetSuccessful) return ResetSuccessful;
            ResetCallCount++;
            return ResetSuccessful;
        }
    }
    
    private readonly ResettablePoolPolicy<TestResettable> _policy = new();
    
    [Test]
    public async Task Reset_ShouldCallReset() {
        // Arrange
        var resettable = new TestResettable();
        
        // Act
        bool result = _policy.Return(resettable);
        
        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(resettable.ResetCallCount).IsEqualTo(1);
    }
    
    [Test]
    public async Task Reset_ShouldReturnFalse_IfResetFails() {
        // Arrange
        var resettable = new TestResettable { ResetSuccessful = false };
        
        // Act
        bool result = _policy.Return(resettable);
        
        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(resettable.ResetCallCount).IsEqualTo(0);
    }

    [Test]
    public async Task Create_ShouldReturnNewInstance() {
        // Arrange
        
        // Act
        TestResettable result = _policy.Create();
        
        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.ResetCallCount).IsEqualTo(0);
    }

}
