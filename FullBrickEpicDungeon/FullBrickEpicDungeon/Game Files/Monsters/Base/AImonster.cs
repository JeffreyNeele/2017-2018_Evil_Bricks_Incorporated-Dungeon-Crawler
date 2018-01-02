using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class AImonster : Monster
{
    protected BaseAI AI;
    public AImonster(Vector2 movementSpeed, Level currentLevel, string type) : base(movementSpeed, type, currentLevel)
    {
        AI = new BaseAI(this, movementSpeed, currentLevel);
    }

    public override void Update(GameTime gameTime)
    {
        AI.Update(gameTime);
        base.Update(gameTime);
    }
}
