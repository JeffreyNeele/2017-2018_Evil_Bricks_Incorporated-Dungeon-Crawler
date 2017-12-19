using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

//State dat een in-game menu weergeeft. In deze state wordt het spel gepauzeerd en kan er een aantal acties worden ondernomen.
class PauseState : GameObjectList
{
    protected Button continueButton, quitButton;

    public PauseState()
    {
        continueButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        continueButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - continueButton.Width / 2), (GameEnvironment.Screen.Y / 2 ));
        Add(continueButton);

        quitButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        quitButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - quitButton.Width / 2), (GameEnvironment.Screen.Y / 2 + quitButton.Height / 2));
        Add(quitButton);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (continueButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        else if (quitButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
    }
}

