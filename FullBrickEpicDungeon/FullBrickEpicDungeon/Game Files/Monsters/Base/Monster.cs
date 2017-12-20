using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// NOTE: the gold attribute for the monster is seen as the amount of gold the player gets when it is defeated.
abstract partial class Monster : SpriteGameObject
{
    protected BaseAttributes attributes, baseattributes;
    public Monster(string assetName, string type) : base(assetName)
    {
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsDead)
        {
            base.Update(gameTime);
        }

    }

    public override void Reset()
    {
        base.Reset();
    }
    //Takes damage, HP can never be below 0 because of health bars
    public void TakeDamage(int damage)
    {
        this.attributes.HP -= (damage - (int)(0.3F * this.attributes.Armour));
        if(this.attributes.HP <= 0)
        {
            this.attributes.HP = 0;
        }
    }

    public bool CollisionChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
        Rectangle quarterBoundingBox = new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y + 0.75 * Height), this.Width, (int)(this.Height / 4));
        foreach (Tile tile in Field.Objects)
        {
            if ((tile.Type == TileType.Brick || tile.Type == TileType.RockIce) && quarterBoundingBox.Intersects(tile.BoundingBox))
            {
                return false;
            }
        }
        return true;
    }

    // returns the base attributes of a monster
    public BaseAttributes Attributes
    {
        get { return attributes; }
        set { attributes = value; }
    }

    public bool IsDead
    {
        get { return this.attributes.HP == 0; }
    }
}