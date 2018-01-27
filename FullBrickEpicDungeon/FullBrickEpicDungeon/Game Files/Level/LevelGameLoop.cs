using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Game loop for the level
/// </summary>
partial class Level : GameObjectList
{
    public override void Update(GameTime gameTime)
    {
        int downedCount = 0;
        foreach(Character c in playerList.Children)
        {
            if (c.IsDowned)
                downedCount++;
        }
        if(downedCount >= 4)
        {
            GameEnvironment.GameStateManager.SwitchTo("gameOverState");
        }


        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
    }

    public override void Reset()
    {
        base.Reset();
    }
}