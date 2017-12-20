using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// NOTE: the gold attribute for the monster is seen as the amount of gold the player gets when it is defeated.
abstract partial class Monster : SpriteGameObject
{
    protected BaseAttributes attributes, baseattributes;
    protected BaseAI baseAI;
    public Monster(Vector2 movementSpeed, string assetName, string type) : base(assetName)
    {
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();

        // Initialilze the AI
        baseAI = new BaseAI(GameWorld.Find("levelTileField") as GameObjectGrid);
        baseAI.Parent = this;
        baseAI.ParentMovementSpeed = movementSpeed;
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