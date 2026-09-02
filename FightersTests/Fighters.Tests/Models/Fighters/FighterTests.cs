using Fighters.Models.Armors;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Roles;
using Fighters.Models.Weapons;

namespace Fighters.Tests.Models.Fighters;

public class FighterTests
{
    [Fact]
    public void Constructor_Valid_Arguments_Initializes_Properties_Correctly()
    {
        // Arrange
        string name = "Slava";
        IRace race = new Human();
        IRole role = new Knight();
        IArmor armor = new NoArmor();
        IWeapon weapon = new Fists();
        int maxHealth = race.Health + role.Health;

        // Act
        Fighter fighter = new Fighter( name, race, armor, weapon, role );

        // Assert
        Assert.True( fighter.IsAlive() );
        Assert.Equal( name, fighter.Name );
        Assert.Equal( maxHealth, fighter.GetMaxHealth() );
        Assert.Equal( maxHealth, fighter.GetCurrentHealth() );
    }

    [Fact]
    public void CalculateDamage_Return_Sum_Of_Race_Role_Weapon_Damages()
    {
        // Arrange
        string name = "Slava";
        IRace race = new Human();
        IRole role = new Knight();
        IArmor armor = new NoArmor();
        IWeapon weapon = new Fists();
        Fighter fighter = new Fighter( name, race, armor, weapon, role );
        int expectedDamage = 5 + 15 + 1;

        // Act
        int result = fighter.CalculateDamage();

        // Assert
        Assert.Equal( expectedDamage, result );
    }

    [Fact]
    public void CalculateArmor_Returns_Sum_Of_Armor_And_Race_Armor()
    {
        // Arrange
        IRace race = new Human();
        IArmor armor = new MetalArmor();
        Fighter fighter = new Fighter( "Slava", race, armor, new Fists(), new Knight() );
        int expectedArmor = 0 + 15;

        // Act
        int result = fighter.CalculateArmor();

        // Assert
        Assert.Equal( expectedArmor, result );
    }

    [Fact]
    public void TakeDamage_Positive_Damage_Update_Health()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        int initialHealth = fighter.GetCurrentHealth();
        int damage = 30;

        // Act
        fighter.TakeDamage( damage );

        // Assert
        Assert.Equal( initialHealth - damage, fighter.GetCurrentHealth() );
        Assert.True( fighter.IsAlive() );
    }

    [Fact]
    public void TakeDamage_Negative_Damage_Throw_Exception()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();

        // Act and Assert
        Assert.Throws<GameBattleException>( () => fighter.TakeDamage( -1 ) );
    }

    [Fact]
    public void TakeDamage_Damage_Greater_Than_Health_Sets_Health_Zero()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        int maxHealth = fighter.GetMaxHealth();

        // Act
        fighter.TakeDamage( maxHealth + 1 );

        // Assert
        Assert.Equal( 0, fighter.GetCurrentHealth() );
        Assert.False( fighter.IsAlive() );
    }

    [Fact]
    public void TakeDamage_Zero_Damage_Not_Change_Health()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        int initialHealth = fighter.GetCurrentHealth();

        // Act
        fighter.TakeDamage( 0 );

        // Assert
        Assert.Equal( initialHealth, fighter.GetCurrentHealth() );
        Assert.True( fighter.IsAlive() );
    }

    [Fact]
    public void TakeDamage_Damage_Equal_To_Health_Set_Health_To_Zero()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        int maxHealth = fighter.GetMaxHealth();

        // Act
        fighter.TakeDamage( maxHealth );

        // Assert
        Assert.Equal( 0, fighter.GetCurrentHealth() );
        Assert.False( fighter.IsAlive() );
    }

    [Fact]
    public void CalculateBaseDamage_Defender_Has_Higher_Armor_Return_Zero()
    {
        // Arrange
        Fighter attacker = new Fighter( "FighterA", new Human(), new NoArmor(), new Fists(), new Healer() );
        Fighter defender = new Fighter( "FighterB", new Human(), new DiamondArmor(), new Fists(), new Knight() );

        // Act
        int baseDamage = attacker.CalculateBaseDamage( defender );

        // Assert
        Assert.Equal( 0, baseDamage );
    }

    [Fact]
    public void CalculateBaseDamage_Defender_Has_Lower_Armor_Return_Positive_Difference()
    {
        // Arrange
        Fighter attacker = new Fighter( "FighterA", new Human(), new NoArmor(), new Gun(), new Knight() );
        Fighter defender = new Fighter( "FighterB", new Human(), new NoArmor(), new Fists(), new Knight() );

        // Act
        int baseDamage = attacker.CalculateBaseDamage( defender );
        int expectedDamage = attacker.CalculateDamage() - defender.CalculateArmor();

        // Assert
        Assert.Equal( expectedDamage, baseDamage );
    }

    [Fact]
    public void IsAlive_When_Health_Max_Return_True()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();

        // Act and Assert
        Assert.True( fighter.IsAlive() );
    }

    [Fact]
    public void IsAlive_When_Health_Greater_Than_Zero_Return_True()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        fighter.TakeDamage( 1 );

        // Act and Assert
        Assert.True( fighter.IsAlive() );
    }

    [Fact]
    public void IsAlive_When_Health_Is_Zero_Return_False()
    {
        // Arrange
        Fighter fighter = CreateDefaultFighter();
        fighter.TakeDamage( fighter.GetMaxHealth() );

        // Act and Assert
        Assert.False( fighter.IsAlive() );
    }

    private static Fighter CreateDefaultFighter()
    {
        return new Fighter(
            "Slava",
            new Human(),
            new NoArmor(),
            new Fists(),
            new Knight()
        );
    }
}