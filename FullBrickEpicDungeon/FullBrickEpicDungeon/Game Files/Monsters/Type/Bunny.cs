using Microsoft.Xna.Framework;

class Bunny : AImonster
{
    public Bunny(Level currentLevel) : base(new Vector2(3, 3), currentLevel, "Bunny")
    {
        this.baseattributes.HP = 50;
        this.baseattributes.Armour = 5;
        this.baseattributes.Attack = 20;
        this.baseattributes.Gold = 50;
        attributes = baseattributes;

    }
}
