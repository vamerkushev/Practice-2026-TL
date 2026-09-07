namespace Fighters.RandomController;

public class RandomControl : IRandomControl
{
    private readonly Random _random = new();

    public int Next( int minValue, int maxValue ) => _random.Next( minValue, maxValue );
    public double NextDouble() => _random.NextDouble();
}