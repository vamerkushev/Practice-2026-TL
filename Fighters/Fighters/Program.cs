using Fighters.Extensions;
using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;

namespace Fighters;

public class Program
{
    public static void Main()
    {
        PrintMenu();

        while ( true )
        {
            Console.Write( "Введите команду: " );
            string input = Console.ReadLine() ?? string.Empty;

            if ( string.IsNullOrEmpty( input ) )
            {
                continue;
            }

            switch ( input )
            {
                case "/add":
                    AddFighter();
                    break;
                case "/list":
                    ShowListFighters();
                    break;
                case "/fight":
                    LaunchFight();
                    break;
                case "/clear":
                    DeleteFighters();
                    break;
                case "/exit":
                    Console.WriteLine( "Игра окорнчена!" );
                    return;
                default:
                    Console.WriteLine( "Неизвестная команда!" );
                    break;
            }
        }
    }

    private static readonly FightManagement _gameManager = new();

    private static readonly List<IRace> Races = new()
    {
        new Human(), new Elf(), new Gnome(), new Goblin(), new Hobbit()
    };

    private static readonly List<IRole> Roles = new()
    {
        new Guardian(), new Healer(), new Knight(), new Ninja(), new Wizard()
    };

    private static readonly List<IWeapon> Weapons = new()
    {
        new Fists(), new Axe(), new Sword(), new Arbalest(), new Gun()
    };

    private static readonly List<IArmor> Armors = new()
    {
        new NoArmor(), new LeatherArmor(), new MetalArmor(), new GoldenArmor(), new DiamondArmor()
    };

    private static void PrintMenu()
    {
        Console.WriteLine( "Добро пожаловать в игру, Бойцы!" );
        Console.WriteLine( "Команды для игры:" );
        Console.WriteLine( "/add - добавить нового бойца" );
        Console.WriteLine( "/list - показать список всех бойцов" );
        Console.WriteLine( "/clear - удалить всех бойцов" );
        Console.WriteLine( "/fight - начать битву" );
        Console.WriteLine( "/exit - выход из игры" );
    }

    private static void AddFighter()
    {
        Console.WriteLine( "Введите имя бойца:" );
        string name = Console.ReadLine() ?? string.Empty;
        while ( string.IsNullOrWhiteSpace( name ) || _gameManager.IsNameOccupied( name ) )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                Console.WriteLine( "Имя не может быть пустым!" );
            }
            else
            {
                Console.WriteLine( $"Боец с именем '{name}' уже создан! Введите другое имя:" );
            }
            name = Console.ReadLine() ?? string.Empty;
        }

        var builder = new Builder( Races, Roles, Weapons, Armors );

        Fighter fighter = builder
            .AddName( name )
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        try
        {
            _gameManager.AddFighter( fighter );
            Console.WriteLine( $"Боец {fighter.Name} добавлен!" );
        }
        catch ( Exception e )
        {
            Console.WriteLine( $"Ошибка: {e.Message}" );
        }
    }

    private static void ShowListFighters()
    {
        var fighters = _gameManager.GetFighters();
        if ( fighters.Count == 0 )
        {
            Console.WriteLine( "Список бойцов пуст!" );
            return;
        }

        Console.WriteLine( "Список бойцов:" );
        foreach ( var f in fighters )
        {
            Console.WriteLine( $"Боец {f.Name}, HP: {f.GetCurrentHealth()} из {f.GetMaxHealth()}, Сила: {f.CalculateDamage()}, Броня: {f.CalculateArmor()}, {( f.IsAlive() ? "Жив" : "Мёртв" )}" );
        }
    }

    private static void LaunchFight()
    {
        try
        {
            _gameManager.RunBattle();
        }
        catch ( Exception e )
        {
            Console.WriteLine( $"Ошибка: {e.Message}" );
        }
    }

    private static void DeleteFighters()
    {
        var fighters = _gameManager.GetFighters();

        if ( fighters.Count == 0 )
        {
            Console.WriteLine( "Список бойцов и так пуст! Для добавления бойца используйте команду /add." );
        }
        else
        {
            Console.WriteLine( "Все бойцы удалены!" );
        }

        _gameManager.DeleteFighters();
    }
}