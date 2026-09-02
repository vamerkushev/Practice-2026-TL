using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;
using Fighters.RandomController;
using Fighters.SystemConsole;
using Fighters.Tests.Mocks;
using Moq;

namespace Fighters.Tests;

public class FightManagementTests
{
    [Fact]
    public void IsNameOccupied_No_Existing_Name_Return_False()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        manager.AddFighter( CreateDefaultFighter( "Slava" ) );

        // Act
        bool exist = manager.IsNameOccupied( "Slava_2" );

        // Assert
        Assert.False( exist );
    }

    [Theory]
    [InlineData( "Slava" )]
    [InlineData( "slava" )]
    [InlineData( "slava " )]
    [InlineData( " slava " )]
    public void IsNameOccupied_Existing_Name_Return_True( string inputName )
    {
        // Arrange
        FightManagement manager = new FightManagement();
        manager.AddFighter( CreateDefaultFighter( "Slava" ) );

        // Act
        bool exist = manager.IsNameOccupied( inputName );

        // Assert
        Assert.True( exist );
    }

    [Fact]
    public void AddFighter_Add_Fighter_To_List()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        Fighter fighter = CreateDefaultFighter();

        // Act
        manager.AddFighter( fighter );
        List<Fighter> fighters = manager.GetFighters();

        // Assert
        Assert.Single( fighters );
        Assert.Same( fighter, fighters[ 0 ] );
    }

    [Fact]
    public void GetFighters_Return_Copy_Of_List()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        manager.AddFighter( CreateDefaultFighter() );

        // Act
        List<Fighter> list1 = manager.GetFighters();
        List<Fighter> list2 = manager.GetFighters();

        // Assert
        Assert.NotSame( list1, list2 );
        Assert.Equal( list1[ 0 ].Name, list2[ 0 ].Name );
    }

    [Fact]
    public void DeleteFighters_Clear_List()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        manager.AddFighter( CreateDefaultFighter() );

        // Act
        manager.DeleteFighters();

        // Assert
        Assert.Empty( manager.GetFighters() );
    }

    [Fact]
    public void RunBattle_Zero_Fighters_Throw_GameBattleException()
    {
        // Arrange
        FightManagement manager = new FightManagement();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => manager.RunBattle() );
    }

    [Fact]
    public void RunBattle_Less_Two_Fighters_Throw_GameBattleException()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        manager.AddFighter( CreateDefaultFighter() );

        // Act and Assert
        Assert.Throws<GameBattleException>( () => manager.RunBattle() );
    }

    [Fact]
    public void RunBattle_Two_Fighters_One_Dies_And_One_Wins()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        Fighter fighter_1 = CreateDefaultFighter( "Fighter_1" );
        Fighter fighter_2 = CreateDefaultFighter( "Fighter_2" );
        manager.AddFighter( fighter_1 );
        manager.AddFighter( fighter_2 );

        // Act
        Fighter winner = manager.RunBattle();

        // Assert
        Fighter loser = ( winner == fighter_1 ) ? fighter_2 : fighter_1;
        Assert.True( winner.IsAlive() );
        Assert.False( loser.IsAlive() );
    }

    [Fact]
    public void RunBattle_More_Two_Fighters_One_Wins_Other_Dies()
    {
        // Arrange
        FightManagement manager = new FightManagement();
        Fighter fighter_1 = CreateDefaultFighter( "Fighter_1" );
        Fighter fighter_2 = CreateDefaultFighter( "Fighter_2" );
        Fighter fighter_3 = CreateDefaultFighter( "Fighter_3" );
        manager.AddFighter( fighter_1 );
        manager.AddFighter( fighter_2 );
        manager.AddFighter( fighter_3 );

        // Act
        Fighter winner = manager.RunBattle();

        // Assert
        Assert.True( winner.IsAlive() );
        int aliveCount = manager.GetFighters().Count( f => f.IsAlive() );
        Assert.Equal( 1, aliveCount );
    }

    [Fact]
    public void RunBattle_With_Mock_Random_Finish_And_Use_Random_Controller()
    {
        // Arrange
        Mock<IRandomControl> randomMock = RandomControllerMockFactory.CreateMock(
            multiplier: 0.9,
            isCritical: false );

        Mock<ISystemConsole> consoleMock = new Mock<ISystemConsole>();

        FightManagement manager = new FightManagement( randomMock.Object, consoleMock.Object );
        Fighter attacker = new Fighter( "Fighter_1", new Human(), new NoArmor(), new Fists(), new Knight() );
        Fighter defender = new Fighter( "Fighter_2", new Human(), new NoArmor(), new Fists(), new Knight() );

        manager.AddFighter( attacker );
        manager.AddFighter( defender );

        // Act
        Fighter winner = manager.RunBattle();

        // Assert
        Assert.True( winner.IsAlive() );
        Assert.False( ( winner == attacker ? defender : attacker ).IsAlive() );

        randomMock.Verify( r => r.Next( It.IsAny<int>(), It.IsAny<int>() ), Times.AtLeastOnce() );
        randomMock.Verify( r => r.NextDouble(), Times.AtLeastOnce() );
    }

    [Fact]
    public void RunBattle_With_Mock_Random_Critical_Hit_Print_Message()
    {
        // Arrange
        Mock<IRandomControl> randomMock = RandomControllerMockFactory.CreateMock(
            multiplier: 0.9,
            isCritical: true );

        Mock<ISystemConsole> consoleMock = new Mock<ISystemConsole>();

        FightManagement manager = new FightManagement( randomMock.Object, consoleMock.Object );
        Fighter attacker = new Fighter( "Fighter_1", new Human(), new NoArmor(), new Fists(), new Knight() );
        Fighter defender = new Fighter( "Fighter_2", new Human(), new NoArmor(), new Fists(), new Knight() );

        manager.AddFighter( attacker );
        manager.AddFighter( defender );

        // Act
        manager.RunBattle();

        // Assert
        consoleMock.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "КРИТИЧЕСКИЙ УДАР" ) ) ), Times.AtLeastOnce() );
    }

    [Fact]
    public void RunBattle_With_Mock_Random_No_Critical_Hit_Not_Print_Message()
    {
        // Arrange
        Mock<IRandomControl> randomMock = RandomControllerMockFactory.CreateMock(
            multiplier: 0.9,
            isCritical: false );

        Mock<ISystemConsole> consoleMock = new Mock<ISystemConsole>();

        FightManagement manager = new FightManagement( randomMock.Object, consoleMock.Object );
        Fighter attacker = new Fighter( "Fighter_1", new Human(), new NoArmor(), new Fists(), new Knight() );
        Fighter defender = new Fighter( "Fighter_2", new Human(), new NoArmor(), new Fists(), new Knight() );

        manager.AddFighter( attacker );
        manager.AddFighter( defender );

        // Act
        manager.RunBattle();

        // Assert
        consoleMock.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "КРИТИЧЕСКИЙ УДАР" ) ) ), Times.Never() );
    }

    private static Fighter CreateDefaultFighter( string name = "Slava" )
    {
        return new Fighter(
            name,
            new Human(),
            new NoArmor(),
            new Fists(),
            new Knight() );
    }
}