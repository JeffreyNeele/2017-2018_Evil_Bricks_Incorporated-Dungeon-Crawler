using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

//State that pauses the game
class PauseState : MenuState
{
    protected PlayingState playingState;
    protected Button continueButton, disconnectController, settingsButton, quitButton;
    protected Texture2D overlay;
    TextGameObject disconnectedText;
    bool voluntaryDisconnect = false;

    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState() : base()
    {
        // find the playing state
        
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;

        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");

        FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;


        disconnectedText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "disconnectedtext")
        {
            Color = Color.White,
            Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect",
            Position = new Vector2(GameEnvironment.Screen.X / 2 - 560, 150)
    };
    }

    protected override void FillButtonList()
    {
        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue");
        buttonList.Add(continueButton);

        disconnectController = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(disconnectController);

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
        if(voluntaryDisconnect)
        {
            for(int controller = 1; controller <= 4; controller++)
            {
                if (inputHelper.ButtonPressed(controller, Buttons.Y))
                {
                    Character.DisconnectController(controller);
                    voluntaryDisconnect = false;
                    disconnectedText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";   
                }
                if (inputHelper.ButtonPressed(controller, Buttons.B))
                {
                    voluntaryDisconnect = false;
                }
            }
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

        if (Character.ControllerNrDisconnected != -1 || voluntaryDisconnect) //als er een controller disconnected is of iemand wil vrijwillig disconnecten
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
                        GameEnvironment.GameStateManager.SwitchTo("playingState");
                        break;
                    case 1: //Disconnect controller button
                        if (Character.ControllerNrDisconnected != -1) //a controller is disconnected
                        {
                            Character.DisconnectController(Character.ControllerNrDisconnected);
                        }
                        else //no controller is disconnected
                        {
                            disconnectedText.Text = "Press interact on the controller you wish to deconnect. Or press B to go back";
                            voluntaryDisconnect = true;
                        }
                        break;
                    case 2: //Settings button pressed
                        GameEnvironment.GameStateManager.SwitchTo("settingsState");
                        break;
                    case 3: //Quit button pressed
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
            if (inputHelper.ControllerConnected(xboxcontroller) && Character.ControllerNrDisconnected == xboxcontroller)
            {
                buttonList[0].Pressed = true; //continue if the controller has reconnected
                ButtonPressedHandler();
                Character.ControllerNrDisconnected = -1; //no controller is disconnected.
            }
        }


    }


}

