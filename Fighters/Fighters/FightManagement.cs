using Fighters.Models.Fighters;

namespace Fighters;

public class FightManagement
{
    private const double ChanceCriticalHit = 0.2;
    private const int MultiplicatorCriticalHit = 2;
    private const double MinLimitRandom = 0.8;
    private const double MaxSpreadRandom = 0.3;

    private readonly List<Fighter> _fighters = [];
    private readonly Random _random = new();

    public Fighter RunBattle()
    {
        List<Fighter> alive = GetAliveFighters();

        if ( alive.Count < 2 )
        {
            throw new GameBattleException( "Для запуска раунда нужно минимум 2 бойца!" );
        }

        int round = 1;

        while ( alive.Count > 1 )
        {
            Console.WriteLine( $"Раунд {round}:" );
            DefineInitiative( alive );

            CalculateResultRound( alive );

            alive = GetAliveFighters();
            round++;
        }
        Fighter winner = alive[ 0 ];
        Console.WriteLine( $"Боец {winner.Name} победил!" );
        return winner;
    }

    public bool IsNameOccupied( string name )
    {
        foreach ( Fighter fighter in _fighters )
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

    private void CalculateResultRound( List<Fighter> alive )
    {
        foreach ( Fighter attacker in alive )
        {
            if ( !attacker.IsAlive() )
            {
                continue;
            }

            Fighter? defender = ChooseDefender( attacker, alive );
            if ( defender == null )
            {
                continue;
            }

            int totalDamage = CalculateTotalDamage( attacker, defender );

            ApplyDamage( attacker, defender, totalDamage );
        }
    }

    private Fighter? ChooseDefender( Fighter attacker, List<Fighter> alive )
    {
        List<Fighter> defenders = [];

        foreach ( Fighter f in alive )
        {
            if ( f != attacker && f.IsAlive() )
            {
                defenders.Add( f );
            }
        }

        return defenders.Count == 0 ? null : defenders[ _random.Next( defenders.Count ) ];
    }

    private int CalculateTotalDamage( Fighter attacker, Fighter defender )
    {
        int calculatedDamage = attacker.CalculateDamage() - defender.CalculateArmor();
        if ( calculatedDamage < 0 )
        {
            calculatedDamage = 0;
        }

        if ( calculatedDamage == 0 )
        {
            calculatedDamage = 1;
        }

        double randomDamageChange = MinLimitRandom + _random.NextDouble() * MaxSpreadRandom;

        int totalDamage = ( int )( calculatedDamage * randomDamageChange );

        if ( _random.NextDouble() <= ChanceCriticalHit )
        {
            totalDamage = totalDamage * MultiplicatorCriticalHit;
            Console.WriteLine( $"Боец {attacker.Name} нанёс КРИТИЧЕСКИЙ УДАР!" );
        }

        return totalDamage;
    }

    private void ApplyDamage( Fighter attacker, Fighter defender, int totalDamage )
    {
        defender.TakeDamage( totalDamage );
        Console.WriteLine( $"Боец {attacker.Name} нанёс {totalDamage} урона бойцу {defender.Name}. У него осталось {defender.GetCurrentHealth()} HP" );

        if ( !defender.IsAlive() )
        {
            Console.WriteLine( $"Боец {defender.Name} погиб!" );
        }
    }

    private List<Fighter> GetAliveFighters()
    {
        List<Fighter> alive = [];
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