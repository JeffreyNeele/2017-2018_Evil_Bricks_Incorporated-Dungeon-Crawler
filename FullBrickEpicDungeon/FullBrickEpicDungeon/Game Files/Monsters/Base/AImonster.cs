using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class AImonster : Monster
{
    protected BaseAI AI;
    public AImonster(float movementSpeed, Level currentLevel, string type) : base(type, currentLevel)
    {
        AI = new BaseAI(this, movementSpeed, currentLevel);
    }

    public override void Update(GameTime gameTime)
    {
        AI.Update(gameTime);
        base.Update(gameTime);
    }
}
