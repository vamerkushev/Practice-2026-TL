namespace Fighters.RandomController;

public interface IRandomControl
{
    int Next( int minValue, int maxValue );
    double NextDouble();
}