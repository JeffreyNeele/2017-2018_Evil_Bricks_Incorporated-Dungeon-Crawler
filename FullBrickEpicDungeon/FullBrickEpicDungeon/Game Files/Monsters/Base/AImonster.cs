using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class AImonster : Monster
{
    protected Vector2 previousPos;
    protected BaseAI AI;
    protected bool attackSide;
    public AImonster(float movementSpeed, Level currentLevel, string type) : base(type, currentLevel)
    {
        AI = new BaseAI(this, movementSpeed, currentLevel);
    }

    public override void Update(GameTime gameTime)
    {
        AI.Update(gameTime);
        if (AI.IsAttacking)
        {
            PlayAnimation("attack");
            if (previousPos.X < position.X)
                this.Mirror = true;
            else if (previousPos.X > position.X)
                this.Mirror = false;
        }
        else if (previousPos.Y > position.Y)
        {
            PlayAnimation("walk_back");
            if (previousPos.X < position.X)
                this.Mirror = false;
            else if (previousPos.X > position.X)
                this.Mirror = true;
        }
        else if (previousPos.X < position.X)
        {
            PlayAnimation("walk");
            this.Mirror = true;
        }
        else if (previousPos.X > position.X)
        {
            PlayAnimation("walk");
            this.Mirror = false;
        }
        else
            PlayAnimation("idle");

        base.Update(gameTime);
    }
    
}
