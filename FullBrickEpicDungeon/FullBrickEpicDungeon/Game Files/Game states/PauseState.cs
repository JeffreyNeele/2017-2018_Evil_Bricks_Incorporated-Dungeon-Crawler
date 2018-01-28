using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

//State that pauses the game
class PauseState : MenuState
{
    protected PlayingState playingState;
    protected CharacterSelection characterSelection;
    protected Button continueButton, settingsButton, quitButton;
    protected Texture2D overlay;
    TextGameObject disconnectedText;

    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState() : base()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        characterSelection = GameEnvironment.GameStateManager.GetGameState("characterSelection") as CharacterSelection;

        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");

        FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;


        disconnectedText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "disconnectedtext")
        {
            Color = Color.White,
            Text = "Controller " + characterSelection.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or:",   
        };
        disconnectedText.Position = new Vector2(GameEnvironment.Screen.X / 2 - 960, 150);
    }

    protected override void FillButtonList()
    {
        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue");
        buttonList.Add(continueButton);

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(settingsButton);

        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu");
        buttonList.Add(quitButton);

        base.FillButtonList(); //gives positions to the buttons and the marker
    }

    public override void Update(GameTime gameTime)
    {
        //playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    protected override void HandleXboxInput(InputHelper inputHelper, int controllernumber)
    {
        base.HandleXboxInput(inputHelper, controllernumber);
        if (inputHelper.ButtonPressed(controllernumber, Buttons.B) || inputHelper.ButtonPressed(controllernumber, Buttons.Start))
        {
            buttonList[0].Pressed = true; //Continue if B is pressed.
            ButtonPressedHandler();
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        CheckControllerConnected(inputHelper);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        //draw the overlay
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);

        if (characterSelection.ControllerNrDisconnected != -1) //als er een controller disconnected is
        {
            disconnectedText.Draw(gameTime, spriteBatch);
        }

        base.Draw(gameTime, spriteBatch);
    }



    protected override void ButtonPressedHandler()
    {
        //check for each button in the buttonlist if it is pressed.
        for (int buttonnr = 0; buttonnr < buttonList.Count; buttonnr++)
        {
            if (buttonList[buttonnr].Pressed)
            {
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");

                switch (buttonnr)
                {
                    case 0: //Continue button pressed
                        (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                        GameEnvironment.GameStateManager.SwitchTo("playingState");
                        break;
                    case 1: //Settings button pressed
                        GameEnvironment.GameStateManager.SwitchTo("settingsState");
                        break;
                    case 2: //Quit button pressed
                        (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).ResetLevelIndex();
                        GameEnvironment.GameStateManager.SwitchTo("titleMenu");
                        break;
                    default: throw new IndexOutOfRangeException("Buttonbehaviour not defined. Buttonnumber in buttonList: " + buttonnr);

                }
            }
        }
    }

    protected void CheckControllerConnected(InputHelper inputHelper)
    {
        for (int xboxcontroller = 1; xboxcontroller <= 4; xboxcontroller++)
        {
            if (inputHelper.ControllerConnected(xboxcontroller) && characterSelection.ControllerNrDisconnected == xboxcontroller)
            {
                buttonList[0].Pressed = true; //continue if the controller has reconnected
                ButtonPressedHandler();
                characterSelection.ControllerNrDisconnected = -1; //no controller is disconnected.
            }
        }


    }


}

