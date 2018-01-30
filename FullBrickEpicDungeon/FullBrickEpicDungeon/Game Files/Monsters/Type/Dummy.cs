using Microsoft.Xna.Framework;

class Dummy : Monster
{
    /// <summary>
    /// Class that defines a Dummy, a monster without AI and that can only receive damage
    /// </summary>
    /// <param name="assetName">The asset path we want to use to load the sprite</param>
    /// <param name="currentLevel">the current level the dummy is in</param>
    public Dummy(string assetName, Level currentLevel)
        : base("dummy", currentLevel)
    {
        // assign the baseattributes (for resetting) and the actual attributes
        this.baseattributes.HP = 500;
        this.baseattributes.Armour = 0;
        this.baseattributes.Gold = 0;
        this.baseattributes.Attack = 0;
        attributes.HP = baseattributes.HP;
        attributes.Armour = baseattributes.Armour;
        attributes.Attack = baseattributes.Attack;
        attributes.Gold = baseattributes.Gold;

        LoadAnimation(assetName, "default", false);
        PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

}




