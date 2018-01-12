using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//State dat een in-game menu weergeeft. In deze state wordt het spel gepauzeerd en kan er een aantal acties worden ondernomen.
class PauseState : IGameLoopObject
{
    protected IGameLoopObject playingState;
    protected Button continueButton, quitButton;

    public PauseState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");

        continueButton = new Button("Assets/Sprites/Menu/StartButton", 99);
        continueButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - continueButton.Width / 2, 250);
        quitButton = new Button("Assets/Sprites/Menu/StartButton", 99);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - quitButton.Width / 2, 500);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        continueButton.Draw(gameTime, spriteBatch);
        quitButton.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        continueButton.HandleInput(inputHelper);
        quitButton.HandleInput(inputHelper);
        if (continueButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
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

