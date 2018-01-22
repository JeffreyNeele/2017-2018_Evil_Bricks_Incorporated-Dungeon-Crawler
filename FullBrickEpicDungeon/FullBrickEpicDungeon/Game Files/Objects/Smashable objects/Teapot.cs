using Microsoft.Xna.Framework;

class Teapot : AImonster
{
    /// <summary>
    /// Method that defines a teapot, a teapot is stationary so it isn't a big class
    /// </summary>
    /// <param name="currentLevel">Current level the bunny is in</param>
    public Teapot(Level currentLevel) : base(100F, currentLevel, "Teapot", 200)
    {
        // Assign attributes (for tracking hp etc.) and baseattributes (for resetting)
        this.baseattributes.HP = 10;
        this.baseattributes.Armour = 0;
        this.baseattributes.Attack = 0;
        this.baseattributes.Gold = 5;
        attributes.HP = baseattributes.HP;
        attributes.Armour = baseattributes.Armour;
        attributes.Attack = baseattributes.Attack;
        attributes.Gold = baseattributes.Gold;
        // Load the teapot sprite
        LoadAnimation("Assets/Sprites/SmashableObjects/teapot_red", "idle", false);
        PlayAnimation("idle");
    }

}
