using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// NOTE: the gold attribute for the monster is seen as the amount of gold the player gets when it is defeated.
abstract partial class Monster : SpriteGameObject
{
    protected BaseAttributes attributes, baseattributes;
    protected BaseAI baseAI;
    protected Vector2 movementSpeed;
    public Monster(Vector2 movementSpeed, string assetName, string type, Level currentLevel) : base(assetName, 5, type)
    {
        attributes = new BaseAttributes();
        baseattributes = new BaseAttributes();
        this.movementSpeed = movementSpeed;
        baseAI = new BaseAI(this, movementSpeed, currentLevel);
        GameObjectList AItracker = currentLevel.Find("AItracker") as GameObjectList;
        AItracker.Add(baseAI);
    }


    public override void Update(GameTime gameTime)
    {
        if (!IsDead)
        {
            baseAI.Update(gameTime);
            base.Update(gameTime);
        }
        else
        {
            if (this.color.A - 10 < 0)
            {
                GameObjectList monsterList = GameWorld.Find("monsterLIST") as GameObjectList;
                monsterList.Remove(this);
            }
                
            else
                this.color.A -= 10;
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