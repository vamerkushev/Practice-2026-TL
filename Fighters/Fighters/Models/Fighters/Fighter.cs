using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public class Fighter : IFighter
{
    private readonly IRace _race;
    private readonly IRole _role;
    private readonly IArmor _armor;
    private readonly IWeapon _weapon;

    private int _currentHealth;

    public string Name { get; private set; }
    public int Initiative { get; set; }

    public Fighter( string name, IRace race, IArmor armor, IWeapon weapon, IRole role )
    {
        Name = name;
        _race = race;
        _armor = armor;
        _weapon = weapon;
        _role = role;

        _currentHealth = GetMaxHealth();
    }

    public int GetCurrentHealth() => _currentHealth;

    public int GetMaxHealth() => _race.Health + _role.Health;

    public int CalculateDamage() => _weapon.Damage + _race.Damage + _role.Damage;

    public int CalculateArmor() => _armor.Armor + _race.Armor;

    public void TakeDamage( int damage )
    {
        int newHealth = _currentHealth - damage;
        if ( newHealth < 0 )
        {
            newHealth = 0;
        }

        _currentHealth = newHealth;
    }

    public int CalculateBaseDamage( IFighter defender )
    {
        int damage = CalculateDamage() - defender.CalculateArmor();
        return damage < 0 ? 0 : damage;
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }
}