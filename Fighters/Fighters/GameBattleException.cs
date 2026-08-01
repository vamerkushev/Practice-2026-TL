namespace Fighters;

internal class GameBattleException : Exception
{
    public GameBattleException( string exceptionMessage ) : base( exceptionMessage ) { }
}