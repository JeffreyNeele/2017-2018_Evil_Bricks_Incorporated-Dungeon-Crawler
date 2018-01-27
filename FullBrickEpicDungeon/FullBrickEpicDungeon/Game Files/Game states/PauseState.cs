using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//State that pauses the game
class PauseState : IGameLoopObject
{
    protected IGameLoopObject playingState;
    protected Button continueButton, quitButton;
    protected Texture2D overlay;

    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");

        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue", 99);
        continueButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - continueButton.Width / 2, 250);
        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu", 99);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - quitButton.Width / 2, 500);
    }

    public void Update(GameTime gameTime)
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        //draw the overlay
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
        // draw the buttons
        continueButton.Draw(gameTime, spriteBatch);
        quitButton.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Handles input for the pause state
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        continueButton.HandleInput(inputHelper);
        quitButton.HandleInput(inputHelper);
        if (continueButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }

        else if (quitButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            playingState.Reset();
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
    }


    public void Initialize() { }

    public void Reset()
    {
        continueButton.Reset();
        quitButton.Reset();
    }
}

