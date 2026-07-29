using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public class Builder
{
    private string? _name;
    private IRace? _race;
    private IArmor? _armor;
    private IWeapon? _weapon;
    private IRole? _role;

    private readonly List<IRace> _races;
    private readonly List<IArmor> _armors;
    private readonly List<IWeapon> _weapons;
    private readonly List<IRole> _roles;

    public Builder(
        List<IRace> races,
        List<IRole> roles,
        List<IWeapon> weapons,
        List<IArmor> armors )
    {
        _races = races;
        _roles = roles;
        _weapons = weapons;
        _armors = armors;
    }

    public Builder AddName( string name )
    {
        if ( string.IsNullOrEmpty( name ) )
        {
            throw new ArgumentException( "Имя не может быть пустым!" );
        }
        _name = name;
        return this;
    }

    public Builder AddRace()
    {
        _race = Select( "Выберите расу: ", _races, race => race.GetType().Name, 0 );
        return this;
    }

    public Builder AddArmor()
    {
        _armor = Select( "Выберите броню: ", _armors, armor => armor.GetType().Name, 0 );
        return this;
    }

    public Builder AddWeapon()
    {
        _weapon = Select( "Выберите оружие: ", _weapons, weapon => weapon.GetType().Name, 0 );
        return this;
    }
    public Builder AddRole()
    {
        _role = Select( "Выберите роль: ", _roles, role => role.GetType().Name, 0 );
        return this;
    }

    public Fighter Build()
    {
        if ( string.IsNullOrEmpty( _name ) )
        {
            throw new Exception( "Имя не задано." );
        }
        if ( _race == null || _role == null || _weapon == null || _armor == null )
        {
            throw new Exception( "Не все компоненты выбраны." );
        }
        return new Fighter( _name, _race, _armor, _weapon, _role );
    }

    private static T Select<T>(
        string title,
        IReadOnlyList<T> items,
        Func<T, string> nameItem,
        int defaultValue = -1 )
    {
        while ( true )
        {
            Console.WriteLine( title );
            for ( int i = 0; i < items.Count; i++ )
            {
                string postfix = ( i == defaultValue ) ? " (по умолчанию)" : "";
                Console.WriteLine( $"{i + 1}. {nameItem( items[ i ] )}{postfix}" );
            }

            string input = Console.ReadLine() ?? string.Empty;
            if ( string.IsNullOrEmpty( input ) && defaultValue >= 0 )
            {
                return items[ defaultValue ];
            }

            if ( int.TryParse( input, out int choice ) && choice >= 1 && choice <= items.Count )
            {
                return items[ choice - 1 ];
            }

            Console.WriteLine( "Неверный ввод. Попробуйте снова." );
        }
    }
}