namespace Fighters.Models.Fighters;

public interface IFighter
{
    string Name { get; }
    int Initiative { get; set; }

    public int GetCurrentHealth();
    public int GetMaxHealth();
    public int CalculateDamage();
    public int CalculateArmor();

    public void TakeDamage( int damage );

    public int CalculateBaseDamage( IFighter defender );

    public bool IsAlive();
}