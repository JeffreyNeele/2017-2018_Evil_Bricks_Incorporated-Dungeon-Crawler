using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class AImonster : Monster
{
    protected BaseAI AI;
    public AImonster(float movementSpeed, Level currentLevel, string type) : base(type, currentLevel)
    {
        AI = new BaseAI(this, movementSpeed, currentLevel, 150);
    }

    public override void Update(GameTime gameTime)
    {
        AI.Update(gameTime);

        if (AI.CurrentTarget == null)
        {
            PlayAnimation("idle");
        }
        else if (AI.Direction.Y > AI.Direction.X)
        {
            if (AI.Direction.Y > 0)
            {
                PlayAnimation("walk");
            }
            else
                PlayAnimation("walk_back");
        }
        else if (AI.Direction.X > 0)
        {
            if (AI.Direction.X > 0)
            {
                this.Mirror = true;
                PlayAnimation("walk");
            }
            else
            {
                PlayAnimation("walk");
                this.Mirror = false;
            }
        }

            

        base.Update(gameTime);
    }
    
}
