using Fighters.RandomController;

namespace Fighters.Tests.Mocks;

public class TestRandomControl : IRandomControl
{
    private readonly Random _random = new Random( 1 );
    private readonly double _nextDoubleValue;

    public TestRandomControl( double nextDoubleValue )
    {
        _nextDoubleValue = nextDoubleValue;
    }

    public int Next( int minValue, int maxValue )
    {
        return _random.Next( minValue, maxValue );
    }

    public double NextDouble()
    {
        return _nextDoubleValue;
    }
}