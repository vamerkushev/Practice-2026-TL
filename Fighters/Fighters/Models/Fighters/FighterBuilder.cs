using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public class FighterBuilder
{
    private static readonly List<IRace> _races = new()
    {
        new Human(), new Elf(), new Gnome(), new Goblin(), new Hobbit()
    };

    private static readonly List<IRole> _roles = new()
    {
        new Guardian(), new Healer(), new Knight(), new Ninja(), new Wizard()
    };

    private static readonly List<IWeapon> _weapons = new()
    {
        new Fists(), new Axe(), new Sword(), new Arbalest(), new Gun()
    };

    private static readonly List<IArmor> _armors = new()
    {
        new NoArmor(), new LeatherArmor(), new MetalArmor(), new GoldenArmor(), new DiamondArmor()
    };

    private string? _name;
    private IRace? _race;
    private IArmor? _armor;
    private IWeapon? _weapon;
    private IRole? _role;

    public FighterBuilder AddName( string name )
    {
        if ( string.IsNullOrEmpty( name ) )
        {
            throw new ArgumentException( "Имя не может быть пустым!" );
        }
        _name = name;
        return this;
    }

    public FighterBuilder AddRace()
    {
        _race = Select( "Выберите расу: ", _races, race => race.GetType().Name );
        return this;
    }

    public FighterBuilder AddArmor()
    {
        _armor = Select( "Выберите броню: ", _armors, armor => armor.GetType().Name );
        return this;
    }

    public FighterBuilder AddWeapon()
    {
        _weapon = Select( "Выберите оружие: ", _weapons, weapon => weapon.GetType().Name );
        return this;
    }
    public FighterBuilder AddRole()
    {
        _role = Select( "Выберите роль: ", _roles, role => role.GetType().Name );
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
        Func<T, string> nameItem )
    {
        while ( true )
        {
            Console.WriteLine( title );
            for ( int i = 0; i < items.Count; i++ )
            {
                string postfix = ( i == 0 ) ? " (по умолчанию)" : "";
                Console.WriteLine( $"{i + 1}. {nameItem( items[ i ] )}{postfix}" );
            }

            string input = Console.ReadLine() ?? string.Empty;
            if ( string.IsNullOrEmpty( input ) )
            {
                return items[ 0 ];
            }

            if ( int.TryParse( input, out int choice ) && choice >= 1 && choice <= items.Count )
            {
                return items[ choice - 1 ];
            }

            Console.WriteLine( "Неверный ввод. Попробуйте снова." );
        }
    }
}