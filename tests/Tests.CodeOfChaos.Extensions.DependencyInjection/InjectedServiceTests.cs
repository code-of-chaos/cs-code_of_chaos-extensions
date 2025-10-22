// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Tests.CodeOfChaos.Extensions.DependencyInjection.Data;

namespace Tests.CodeOfChaos.Extensions.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InjectedServicesTest {
    [Test]
    public async Task Test_DuckyService() {
        // Arrange
        var services = new ServiceCollection();
        services.RegisterServicesFromTestsCodeOfChaosExtensionsDependencyInjection();
        ServiceProvider provider = services.BuildServiceProvider();

        // Act
        var duckyService = provider.GetService<IDuckyService>();
        var viewerDucky = provider.GetKeyedService<IKeyedDucky>("viewer");
        var streamerDucky = provider.GetKeyedService<IKeyedDucky>("streamer");
        
        // Assert
        await Assert.That(duckyService).IsNotNull()
            .And.IsTypeOf<DuckyService>();
        
        await Assert.That(viewerDucky).IsNotNull()
            .And.IsTypeOf<ViewerKeyedDucky>()
            .And.HasProperty(d => d.ChaoticFactor).IsEqualTo(5);

        await Assert.That(streamerDucky).IsNotNull()
            .And.IsTypeOf<StreamerKeyedDucky>()
            .And.HasProperty(d => d.ChaoticFactor).IsEqualTo(10);

    }
}
