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
    protected PlayingState playingState;
    Texture2D overlay, plaque;
    TextGameObject levelTimeDisplay;
    public LevelFinishedState()
    {
        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Level_Finished/brown_overlay");
        plaque = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Level_Finished/level_completed");
        playingState = (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState);
        levelTimeDisplay = new TextGameObject("Assets/Fonts/ConversationFont")
        {
            Text = "Time: " + playingState.LevelTime.Minutes.ToString() + " : " + playingState.LevelTime.Seconds.ToString(),
            Position = new Vector2(GameEnvironment.Screen.X / 2 + 270, 760),
            Color = Color.Black
        };
    }

    public void HandleInput(InputHelper inputHelper)
    {
        if (inputHelper.KeyPressed(Keys.E) || inputHelper.AnyPlayerPressed(Buttons.Y))
        {
            (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).GoToNextLevel();
            (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
            GameEnvironment.GameStateManager.SwitchTo("playingState");

        }
    }

    public void Update(GameTime gameTime)
    {
        levelTimeDisplay.Update(gameTime);
        playingState = (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
        spriteBatch.Draw(plaque, Vector2.Zero, Color.White);
        levelTimeDisplay.Draw(gameTime, spriteBatch);
    }

    public void Initialize() { }

    public void Reset()
    {
        levelTimeDisplay.Reset();
        levelTimeDisplay.Text = "Time: " + playingState.LevelTime.Minutes.ToString() + " : " + playingState.LevelTime.Seconds.ToString();
    }
}

