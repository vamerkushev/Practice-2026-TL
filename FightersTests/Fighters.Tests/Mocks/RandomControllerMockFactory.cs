using Moq;
using Fighters.RandomController;

namespace Fighters.Tests.Mocks;

public static class RandomControllerMockFactory
{
    public static Mock<IRandomControl> CreateMock(
        double multiplier = 1.0,
        bool isCritical = false )
    {
        Mock<IRandomControl> mock = new Mock<IRandomControl>();
        int nextCallCount = 0;

        mock.Setup( r => r.Next( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( ( int min, int max ) =>
            {
                int result = min + ( nextCallCount % ( max - min ) );
                nextCallCount++;
                return result;
            } );

        int nextDoubleCallCount = 0;
        mock.Setup( r => r.NextDouble() )
            .Returns( () =>
            {
                nextDoubleCallCount++;
                if ( nextDoubleCallCount == 1 )
                {
                    return multiplier;
                }
                if ( nextDoubleCallCount == 2 )
                {
                    return isCritical ? 0.1 : 0.5;
                }
                return 0.5;
            } );

        return mock;
    }
}