using Fighters.Extensions;
using Fighters.Models.Fighters;

namespace Fighters;

public class FightManagement
{
    private readonly List<Fighter> _fighters = new();
    private readonly Random _random = new();

    public bool IsNameOccupied( string name )
    {
        foreach ( var fighter in _fighters )
        {
            if ( fighter.Name.Equals( name.Trim(), StringComparison.OrdinalIgnoreCase ) )
            {
                return true;
            }
        }
        return false;
    }

    public void AddFighter( Fighter fighter )
    {
        _fighters.Add( fighter );
    }

    public List<Fighter> GetFighters()
    {
        return new List<Fighter>( _fighters );
    }

    public void DeleteFighters()
    {
        _fighters.Clear();
    }

    public Fighter RunBattle()
    {
        const double chanceCriticalHit = 0.2;
        const int multiplicatorCriticalHit = 2;

        const double minLimitRandom = 0.8;
        const double maxSpreadRandom = 0.3;

        List<Fighter> alive = GetAliveFighters();

        if ( alive.Count < 2 )
        {
            throw new Exception( "Для запуска раунда нужно минимум 2 бойца!" );
        }

        int round = 1;

        while ( alive.Count > 1 )
        {
            Console.WriteLine( $"Раунд {round}:" );
            DefineInitiative( alive );

            foreach ( Fighter attacker in alive )
            {
                if ( !attacker.IsAlive() )
                {
                    continue;
                }

                List<Fighter> defenders = new List<Fighter>();

                foreach ( Fighter f in alive )
                {
                    if ( f != attacker && f.IsAlive() )
                    {
                        defenders.Add( f );
                    }
                }

                if ( defenders.Count == 0 )
                {
                    break;
                }

                Fighter defender = defenders[ _random.Next( defenders.Count ) ];

                int calculateDamage = attacker.CalculateDamage() - defender.CalculateArmor();
                if ( calculateDamage < 0 )
                {
                    calculateDamage = 0;
                }

                double randomDamageChange = minLimitRandom + _random.NextDouble() * maxSpreadRandom;

                int totalDamage = ( int )( calculateDamage * randomDamageChange );

                if ( _random.NextDouble() <= chanceCriticalHit )
                {
                    totalDamage = totalDamage * multiplicatorCriticalHit;
                    Console.WriteLine( $"Боец {attacker.Name} нанёс КРИТИЧЕСКИЙ УДАР!" );
                }

                defender.TakeDamage( totalDamage );
                Console.WriteLine( $"Боец {attacker.Name} нанёс {totalDamage} урона бойцу {defender.Name}. У него осталось {defender.GetCurrentHealth()} HP" );

                if ( !defender.IsAlive() )
                {
                    Console.WriteLine( $"Боец {defender.Name} погиб!" );
                }
            }
            alive = GetAliveFighters();
            round++;
        }
        Fighter winner = alive[ 0 ];
        Console.WriteLine( $"Боец {winner.Name} победил!" );
        return winner;
    }

    private List<Fighter> GetAliveFighters()
    {
        List<Fighter> alive = new List<Fighter>();
        foreach ( Fighter f in _fighters )
        {
            if ( f.IsAlive() )
            {
                alive.Add( f );
            }
        }
        return alive;
    }

    private void DefineInitiative( List<Fighter> allFighters )
    {
        SetInitiativeFighters( allFighters );
        allFighters.Sort( ( x, y ) => x.Initiative.CompareTo( y.Initiative ) );
    }

    private void SetInitiativeFighters( List<Fighter> allFighters )
    {
        int countFighter = allFighters.Count;
        HashSet<int> uniqueInitiatives = new HashSet<int>();
        int initiative = _random.Next( 0, countFighter );

        for ( int i = 0; i < countFighter; i++ )
        {
            while ( uniqueInitiatives.Contains( initiative ) )
            {
                initiative = _random.Next( 0, countFighter );
            }
            uniqueInitiatives.Add( initiative );
            allFighters[ i ].Initiative = initiative;
        }
    }
}