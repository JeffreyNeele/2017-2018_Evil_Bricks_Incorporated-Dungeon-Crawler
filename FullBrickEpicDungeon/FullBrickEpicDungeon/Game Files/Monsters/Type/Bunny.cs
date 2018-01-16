using Microsoft.Xna.Framework;

class Bunny : AImonster
{
    public Bunny(Level currentLevel) : base(100F, currentLevel, "Bunny")
    {
        this.baseattributes.HP = 50;
        this.baseattributes.Armour = 5;
        this.baseattributes.Attack = 50;
        this.baseattributes.Gold = 50;
        attributes.HP = baseattributes.HP;
        attributes.Armour = baseattributes.Armour;
        attributes.Attack = baseattributes.Attack;
        attributes.Gold = baseattributes.Gold;
        LoadAnimation("Assets/Sprites/Enemies/bunny_default", "idle", false);
        LoadAnimation("Assets/Sprites/Enemies/bunny_attack@5", "attack", false, 0.2F);
        LoadAnimation("Assets/Sprites/Enemies/rabbit_walk@4", "walk", true, 0.2F);
        LoadAnimation("Assets/Sprites/Enemies/rabbit_walk_back@4", "walk_back", true, 0.2F);
        PlayAnimation("idle");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
