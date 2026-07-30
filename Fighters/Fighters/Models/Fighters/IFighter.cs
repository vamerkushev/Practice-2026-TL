using Fighters.Models.Armors;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public interface IFighter
{
    string Name { get; }
    int Initiative { get; set; }

    public int GetCurrentHealth();
    public int GetMaxHealth();
    public int CalculateDamage();
    public int CalculateArmor();

    public void SetArmor( IArmor armor );
    public void SetWeapon( IWeapon weapon );

    public void TakeDamage( int damage );

    public bool IsAlive();
}