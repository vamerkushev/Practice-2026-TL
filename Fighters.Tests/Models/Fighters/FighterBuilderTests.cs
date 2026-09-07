using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;
using Fighters.SystemConsole;
using Moq;

namespace Fighters.Tests.Models.Fighters;

public class FighterBuilderTests
{
    private static Mock<ISystemConsole> CreateConsoleMockWithInputs( params string[] inputs )
    {
        Mock<ISystemConsole> mock = new Mock<ISystemConsole>();
        var sequence = mock.SetupSequence( c => c.ReadLine() );
        foreach ( string input in inputs )
        {
            sequence = sequence.Returns( input );
        }
        return mock;
    }

    [Fact]
    public void Constructor_Initialize_Builder_With_Default_Values()
    {
        // Arrange
        FighterBuilder builder = new FighterBuilder();

        // Act and Assert
        Assert.NotNull( builder );
    }

    [Fact]
    public void AddName_Valid_Name_Set_Name_And_Return_Builder()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        string expectedName = "Slava";

        // Act
        Fighter fighter = builder
            .AddName( expectedName )
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.Equal( expectedName, fighter.Name );
    }

    [Theory]
    [InlineData( null )]
    [InlineData( "" )]
    [InlineData( " " )]
    public void AddName_Invalid_Name_Throw_GameBattleException( string? invalidName )
    {
        // Arrange
        Mock<ISystemConsole> consoleMock = new Mock<ISystemConsole>();
        FighterBuilder builder = new FighterBuilder();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.AddName( invalidName! ) );
    }

    [Theory]
    [InlineData( "1", typeof( Human ) )]
    [InlineData( "2", typeof( Elf ) )]
    [InlineData( "3", typeof( Gnome ) )]
    [InlineData( "4", typeof( Goblin ) )]
    [InlineData( "5", typeof( Hobbit ) )]
    public void AddRace_Valid_Number_Select_Correct_Race( string input, Type expectedRaceType )
    {
        // Arrange
        string[] inputs = { input, "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType( expectedRaceType, fighter.Race );
    }

    [Fact]
    public void AddRace_Empty_Input_Select_Default_Race()
    {
        // Arrange
        string[] inputs = { "", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType<Human>( fighter.Race );
    }

    [Fact]
    public void AddRace_Invalid_Input_Retry_And_Then_Select_Default()
    {
        // Arrange
        string[] inputs = { "0", "", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType<Human>( fighter.Race );
    }

    // Далее для остальных параметров: роли, брони, оружия не стал писать тесты на проверку невалидных вводов, как выше для расы.
    // Считаю, что это не имеет смысла, так как методы Add.. абсолютно одинаковые, и закономерно результаты тестов будут аналогичные.

    [Theory]
    [InlineData( "1", typeof( Guardian ) )]
    [InlineData( "2", typeof( Healer ) )]
    [InlineData( "3", typeof( Knight ) )]
    [InlineData( "4", typeof( Ninja ) )]
    [InlineData( "5", typeof( Wizard ) )]
    public void AddRole_Valid_Number_Select_Correct_Role( string input, Type expectedRoleType )
    {
        // Arrange
        string[] inputs = { "1", input, "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType( expectedRoleType, fighter.Role );
    }

    [Theory]
    [InlineData( "1", typeof( Fists ) )]
    [InlineData( "2", typeof( Axe ) )]
    [InlineData( "3", typeof( Sword ) )]
    [InlineData( "4", typeof( Arbalest ) )]
    [InlineData( "5", typeof( Gun ) )]
    public void AddWeapon_Valid_Number_Select_Correct_Weapon( string input, Type expectedWeaponType )
    {
        // Arrange
        string[] inputs = { "1", "1", input, "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType( expectedWeaponType, fighter.Weapon );
    }

    [Theory]
    [InlineData( "1", typeof( NoArmor ) )]
    [InlineData( "2", typeof( LeatherArmor ) )]
    [InlineData( "3", typeof( MetalArmor ) )]
    [InlineData( "4", typeof( GoldenArmor ) )]
    [InlineData( "5", typeof( DiamondArmor ) )]
    public void AddArmor_Valid_Number_Select_Correct_Armor( string input, Type expectedArmorType )
    {
        // Arrange
        string[] inputs = { "1", "1", "1", input };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );

        // Act
        Fighter fighter = builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor()
            .Build();

        // Assert
        Assert.IsType( expectedArmorType, fighter.Armor );
    }

    [Fact]
    public void Build_Name_Missing_Throw_GameBattleException()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder
            .AddRace()
            .AddRole()
            .AddWeapon()
            .AddArmor();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.Build() );
    }

    [Fact]
    public void Build_Race_Missing_Throw_GameBattleException()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );
        builder
            .AddRole()
            .AddWeapon()
            .AddArmor();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.Build() );
    }

    [Fact]
    public void Build_Role_Missing_Throw_GameBattleException()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );
        builder
            .AddRace()
            .AddWeapon()
            .AddArmor();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.Build() );
    }

    [Fact]
    public void Build_Weapon_Missing_Throw_GameBattleException()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );
        builder
            .AddRace()
            .AddRole()
            .AddArmor();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.Build() );
    }

    [Fact]
    public void Build_Armor_Missing_Throw_GameBattleException()
    {
        // Arrange
        string[] inputs = { "1", "1", "1", "1" };
        Mock<ISystemConsole> consoleMock = CreateConsoleMockWithInputs( inputs );
        FighterBuilder builder = new FighterBuilder( consoleMock.Object );
        builder.AddName( "Slava" );
        builder
            .AddRace()
            .AddRole()
            .AddWeapon();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => builder.Build() );
    }
}