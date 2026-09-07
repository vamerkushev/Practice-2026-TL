namespace Fighters;

public class GameBattleException : Exception
{
    public GameBattleException( string exceptionMessage ) : base( exceptionMessage ) { }
}