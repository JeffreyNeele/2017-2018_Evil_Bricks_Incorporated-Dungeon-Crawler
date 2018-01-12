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
    protected SpriteGameObject overlay;

    public PauseState()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");

        overlay = new SpriteGameObject("Assets/Sprites/Paused/overlay", 3);
        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue", 99);
        continueButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - continueButton.Width / 2, 250);
        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu", 99);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - quitButton.Width / 2, 500);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        //draw the overlay
        overlay.Draw(gameTime, spriteBatch);
        // draw the buttons
        continueButton.Draw(gameTime, spriteBatch);
        quitButton.Draw(gameTime, spriteBatch);
    }

    // handles input
    public void HandleInput(InputHelper inputHelper)
    {
        continueButton.HandleInput(inputHelper);
        quitButton.HandleInput(inputHelper);
        if (continueButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        // if the quit button is pressed, we reset the playing state and return to the menu
        else if (quitButton.Pressed)
        {
            playingState.Reset();
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
    }

    public void Reset()
    {

    }
}

