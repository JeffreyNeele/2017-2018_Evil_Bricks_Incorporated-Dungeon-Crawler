using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class LevelFinishedState : IGameLoopObject
{
    protected IGameLoopObject playingState;
    Texture2D overlay, plaque;
    public LevelFinishedState()
    {
        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Level_Finished/brown_overlay");
        plaque = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Level_Finished/level_completed");
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    public void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.E) || inputHelper.AnyPlayerPressed(Buttons.Y))
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
            (GameEnvironment.GameStateManager.CurrentGameState as PlayingState).GoToNextLevel();
        }
    }

    public void Update(GameTime gameTime)
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
        spriteBatch.Draw(plaque, Vector2.Zero, Color.White);
    }

    public void Setup() { }

    public void Reset()
    {

    }
}

