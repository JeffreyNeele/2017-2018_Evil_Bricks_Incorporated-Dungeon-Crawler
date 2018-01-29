using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;


//State that pauses the game
class PauseState : MenuState
{
    protected PlayingState playingState;
    protected Button continueButton, resetButton, disconnectController, connectController, settingsButton, controlsButton, quitButton;
    protected Texture2D overlay;
    enum ConnectionStates { Disconnected, VoluntaryDisconnect, Connect};
    int ConnectionState = 0;

    TextGameObject connectText;

    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState() : base()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;

        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");

        FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;


        connectText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "disconnectedtext")
        {
            Color = Color.White,
            Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect",
        };
    }




    protected override void FillButtonList()
    {
        buttonSeparation = 150;

        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue");
        buttonList.Add(continueButton);
        continueButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - buttonList[0].Width / 2, 250);

        resetButton = new Button("Assets/Sprites/Paused/reset_level");
        buttonList.Add(resetButton);

        disconnectController = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(disconnectController);

        connectController = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(connectController);

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(settingsButton);

        controlsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(controlsButton);

        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu");
        buttonList.Add(quitButton);


        //set button positions
        for (int i = 1; i < 4; i++)
        {
            buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - buttonList[0].Width / 2 - 200, 250 + i * buttonSeparation);
        }
        for (int i = 4; i < 7; i++)
        {
            buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - buttonList[0].Width / 2 + 200, 250 + (i-3) * buttonSeparation);
        }

        offsetMarker = new Vector2(-marker.Width, buttonList[0].Height / 2 - marker.Height / 2);
        marker.Position = new Vector2(buttonList[0].Position.X + offsetMarker.X, buttonList[0].Position.Y + offsetMarker.Y);
    }





    public override void Update(GameTime gameTime)
    {
        if(ConnectionState == (int)ConnectionStates.Disconnected)
        {
            connectText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";
            connectText.Position = new Vector2(GameEnvironment.Screen.X / 2 - 670, 200);
        }
        else if(ConnectionState == (int)ConnectionStates.Connect)
        {
            connectText.Text = "Press interact on a keyboard or Xbox Controller to join that controller. Or Xbox press B Keyboard press Shift to go back";
            connectText.Position = new Vector2(GameEnvironment.Screen.X / 2 - 835, 200);
        }
        else if (ConnectionState == (int)ConnectionStates.VoluntaryDisconnect)
        {
            connectText.Text = "Press interact on the controller you wish to disconnect. Or Xbox press B Keyboard press Shift to go back";
            connectText.Position = new Vector2(GameEnvironment.Screen.X / 2 - 740, 200);
        }
    }






    protected override void HandleKeyboardInput(InputHelper inputHelper)
    {
        //moves the marker up or down depending on the key that was pressed, the Dpad or the Thumbstick.
        if (inputHelper.KeyPressed(Keys.Down) || inputHelper.KeyPressed(Keys.S))
        {
            if (buttonIndex < buttonList.Count - 1)
            {
                buttonIndex++;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
        }
        else if (inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.W))
        {
            if (buttonIndex == 4)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex > 0)
            {
                buttonIndex--;
            }
            marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
        }
        else if (inputHelper.KeyPressed(Keys.Right) || inputHelper.KeyPressed(Keys.D))
        {
            if (buttonIndex > 0 && buttonIndex < 4) //left buttons
            {
                buttonIndex += 3;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
            else if (buttonIndex >= 4 && buttonIndex < 7)
            {
                //do nothing, since it is already in the right row
            }
        }
        else if (inputHelper.KeyPressed(Keys.Left) || inputHelper.KeyPressed(Keys.A))
        {
            if (buttonIndex > 0 && buttonIndex < 4) //left buttons
            {
                //do nothing, since it is already in the left row.
            }
            else if (buttonIndex >= 4 && buttonIndex < 7)
            {
                buttonIndex -= 3;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
        }


        if (ConnectionState == (int)ConnectionStates.VoluntaryDisconnect)
        {
                if (inputHelper.KeyPressed(Keys.E))
                {
                    Character.DisconnectController(0);
                    ConnectionState = 0;
                }
                else if(inputHelper.KeyPressed(Keys.L))
                {
                Character.DisconnectController(1);
                ConnectionState = 0;
                }
                else if (inputHelper.KeyPressed(Keys.LeftShift)|| inputHelper.KeyPressed(Keys.RightShift))
                {
                ConnectionState = 0;
                }
        }
        if (ConnectionState == (int)ConnectionStates.Connect) //a controller pressed connect
        {
            if (inputHelper.KeyPressed(Keys.E))
            {
                if (Character.ControllerConnected(0) == false)
                {
                    Character.ConnectController(0);
                    ConnectionState = 0;
                }
            }
            else if (inputHelper.KeyPressed(Keys.L))
            {
                if (Character.ControllerConnected(1) == false)
                {
                    Character.ConnectController(1);
                    ConnectionState = 0;
                }
            }
            else if (inputHelper.KeyPressed(Keys.LeftShift) || inputHelper.KeyPressed(Keys.RightShift))
            {
                ConnectionState = 0;
            }
        }
        if (inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.Enter))
        {
            PressButton();
        }
    }

    protected override void HandleXboxInput(InputHelper inputHelper, int controllernumber)
    {
        //moves the marker up or down depending on the key that was pressed, the Dpad or the Thumbstick.
        if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadDown) || inputHelper.MenuDirection(controllernumber, false, true).Y < 0)
        {
            if (buttonIndex < buttonList.Count - 1)
            {
                buttonIndex++;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
        }
        else if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadUp) || inputHelper.MenuDirection(controllernumber, false, true).Y > 0)
        {
            if (buttonIndex == 4)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex > 0)
            {
                buttonIndex--;
            }
            marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
        }
        else if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadRight) || inputHelper.MenuDirection(controllernumber, true, false).X > 0)
        {
            if (buttonIndex > 0 && buttonIndex < 4) //left buttons
            {
                buttonIndex += 3;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
            else if (buttonIndex >= 4 && buttonIndex < 7)
            {
                //do nothing, since it is already in the right row
            }
        }
        else if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadLeft) || inputHelper.MenuDirection(controllernumber, true, false).X < 0)
        {
            if (buttonIndex > 0 && buttonIndex < 4) //left buttons
            {
                //do nothing, since it is already in the left row.
            }
            else if (buttonIndex >= 4 && buttonIndex < 7)
            {
                buttonIndex -= 3;
                marker.Position = new Vector2(buttonList[buttonIndex].Position.X + offsetMarker.X, buttonList[buttonIndex].Position.Y + offsetMarker.Y);
            }
        }
        


        if (inputHelper.ButtonPressed(controllernumber, Buttons.A))
        {
            PressButton();
        }


        if (inputHelper.ButtonPressed(controllernumber, Buttons.B) || inputHelper.ButtonPressed(controllernumber, Buttons.Start))
        {
            buttonList[0].Pressed = true; //Continue if B is pressed.
            ButtonPressedHandler();
        }
        if(ConnectionState == (int)ConnectionStates.VoluntaryDisconnect)
        {
            for(int controller = 1; controller <= 4; controller++)
            {
                if (inputHelper.ButtonPressed(controller, Buttons.Y))
                {
                    Character.DisconnectController(controller+1);
                    ConnectionState = 0;
                    connectText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";   
                }
                if (inputHelper.ButtonPressed(controller, Buttons.B))
                {
                    connectText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";
                    ConnectionState = 0;
                }
            }
        }
        if (ConnectionState == (int)ConnectionStates.Connect) //a controller pressed connect
        {
            for (int controller = 1; controller <= 4; controller++)
            {
                if (inputHelper.ButtonPressed(controller, Buttons.A))
                {
                    if (Character.ControllerConnected(controller+1) == false)
                    {
                        Character.ConnectController(controller + 1);
                        ConnectionState = 0;
                        connectText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";
                    }
                }
                if (inputHelper.ButtonPressed(controller, Buttons.B))
                {
                    connectText.Text = "Controller " + Character.ControllerNrDisconnected + " has disconnected, reconnect the controller to continue or select disconnect";
                    ConnectionState = 0;
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

        //als er een controller disconnected is of iemand wil vrijwillig disconnecten of connecten
        if (Character.ControllerNrDisconnected != -1 || ConnectionState == (int)ConnectionStates.VoluntaryDisconnect || ConnectionState == (int)ConnectionStates.Connect) 
        {
            connectText.Draw(gameTime, spriteBatch);
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
                    case 1: //reset level pressed
                        (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                        GameEnvironment.GameStateManager.SwitchTo("playingState");
                        break;
                    case 2: //Disconnect controller button
                        if (Character.ControllerNrDisconnected != -1) //a controller is disconnected
                        {
                            Character.DisconnectController(Character.ControllerNrDisconnected);
                        }
                        else //no controller is disconnected
                        {
                            ConnectionState = (int)ConnectionStates.VoluntaryDisconnect;
                        }
                        break;
                    case 3: //connect controller pressed
                        ConnectionState = (int)ConnectionStates.Connect;
                        break;
                    case 4: //Settings button pressed
                        GameEnvironment.GameStateManager.SwitchTo("settingsState");
                        break;
                    case 5:
                        GameEnvironment.GameStateManager.SwitchTo("controlsInfoState");
                        break;
                    case 6: //Quit button pressed
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

    public override void Initialize()
    {
        base.Initialize();
        ConnectionState = 0;
        ConnectionState = 0;
    }


}

