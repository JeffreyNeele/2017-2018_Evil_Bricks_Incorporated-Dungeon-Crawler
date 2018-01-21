using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class LevelFinishedState : GameObjectList
{
    protected IGameLoopObject playingState;
    public LevelFinishedState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.E))
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
            (playingState as PlayingState).GoToNextLevel();
        }
    }

    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}

