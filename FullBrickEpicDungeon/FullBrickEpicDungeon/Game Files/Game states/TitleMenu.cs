using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

class TitleMenuState : IGameLoopObject
{
    protected List<Button> buttonList = new List<Button>();
    Button startButton, settingsButton, quitButton;
    Texture2D background;

    protected SpriteGameObject marker;

    protected Vector2 offsetMarker;
    protected int buttonSeparation = 250;

    //toetsenbord controls dictionary van player 0 en 1 in de dictionary hiervoor
    protected Dictionary<Keys, Keys> keyboardControls1;
    protected Dictionary<Keys, Keys> keyboardControls2;

    //matches the player number to the controls dictionary used.
    protected Dictionary<int, Dictionary<Keys, Keys>> keyboardcontrols = new Dictionary<int, Dictionary<Keys, Keys>>();


    /// <summary>
    /// Class that defines the Title Menu of the game
    /// </summary>
    public TitleMenuState()
    {
        // load the background
        background = GameEnvironment.AssetManager.GetSprite("Assets/Cutscenes/LarryShits");

        marker = new SpriteGameObject("Assets/Sprites/Conversation Boxes/arrow", 1, "", 10, false);

        keyboardControls1 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        keyboardControls2 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        keyboardcontrols.Add(0, keyboardControls1);
        keyboardcontrols.Add(1, keyboardControls2);

        FillButtonList();
    }


    private void FillButtonList()
    {
        // load a settings and start button
        startButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        buttonList.Add(startButton);
        //startButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - startButton.Width / 2), (GameEnvironment.Screen.Y * 3 / 4 - startButton.Height / 2));

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(settingsButton);
        //settingsButton.Position = new Vector2(GameEnvironment.Screen.X - settingsButton.Width, 0);

        quitButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(quitButton);

        //set button positions
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - startButton.Width / 2, 250 + i * buttonSeparation);
        }

        offsetMarker = new Vector2(-marker.Width, startButton.Height / 2 - marker.Height/2);
        marker.Position = new Vector2(startButton.Position.X + offsetMarker.X, startButton.Position.Y + offsetMarker.Y);
    }



    /// <summary>
    /// Draws the title menu
    /// </summary>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, Vector2.Zero, Color.White);

        foreach(Button button in buttonList)
        {
            button.Draw(gameTime, spriteBatch);
        }

        marker.Draw(gameTime, spriteBatch);
    }

    public void Update(GameTime gameTime)
    {

    }

    /// <summary>
    /// Handles the input for the title menu
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        HandleMouseInput(inputHelper);
        HandleKeyboardInput(inputHelper);

        //each connected controller can control the menu
        for (int controllernumber = 1; controllernumber <= 4; controllernumber++)
        {
            if (inputHelper.ControllerConnected(controllernumber))
            {
                HandleXboxInput(inputHelper, controllernumber);
            }
        }
    }


    private void HandleXboxInput(InputHelper inputHelper, int controllernumber)
    {
        //moves the marker up or down depending on the key that was pressed, the Dpad or the Thumbstick.
        if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadDown) || inputHelper.MenuDirection(controllernumber, false, true).Y < 0)
        {
            if (marker.Position.Y < buttonList[buttonList.Count - 1].Position.Y + offsetMarker.Y)
            {
                marker.Position += new Vector2(0, buttonSeparation);
            }
        }
        else if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadUp) || inputHelper.MenuDirection(controllernumber, false, true).Y > 0)
        {
            if (marker.Position.Y > buttonList[0].Position.Y + offsetMarker.Y)
            {
                marker.Position -= new Vector2(0, buttonSeparation);
            }
        }
        if(inputHelper.ButtonPressed(controllernumber, Buttons.A))
        {
            PressButton();
        }
    }

    private void HandleKeyboardInput(InputHelper inputHelper)
    {
        //moves the marker up or down depending on the key that was pressed, the Dpad or the Thumbstick.
        if (inputHelper.KeyPressed(Keys.Down))
        {
            if (marker.Position.Y < buttonList[buttonList.Count - 1].Position.Y + offsetMarker.Y)
            {
                marker.Position += new Vector2(0, buttonSeparation);
            }

        }
        else if (inputHelper.KeyPressed(Keys.Up))
        {
            if (marker.Position.Y > buttonList[0].Position.Y + offsetMarker.Y)
            {
                marker.Position -= new Vector2(0, buttonSeparation);
            }
        }
        if (inputHelper.KeyPressed(Keys.Space))
        {
            PressButton();
        }
    }

    private void HandleMouseInput(InputHelper inputHelper)
    {
        // Updates the input for the start and settingsbutton
        startButton.HandleInput(inputHelper);
        settingsButton.HandleInput(inputHelper);
        //Pressed automatically becomes true when someone clicks the button.

        ButtonPressedHandler();
    }

    public void PressButton()
    {
        for (int index = 0; index < buttonList.Count; index++)
        {
            if (marker.Position.Y == buttonList[index].Position.Y + offsetMarker.Y)
            {
                switch (index)
                {
                    case 0:
                        startButton.Pressed = true;
                        break;
                    case 1:
                        settingsButton.Pressed = true;
                        break;
                    case 2:
                        quitButton.Pressed = true;
                        break;
                        //Pressed wordt automatisch weer op false gezet door de ButtonInputHandler van de muis.
                }

                ButtonPressedHandler();
            }
        }
    }

    public void ButtonPressedHandler()
    {
            if (startButton.Pressed)
            {
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
                FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
                GameEnvironment.GameStateManager.SwitchTo("characterSelection");
            }
            if (settingsButton.Pressed)
            {
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
                GameEnvironment.GameStateManager.SwitchTo("settingsState");
            }
    }

    public void Initialize() { }

    public void Reset()
    {
        startButton.Reset();
    }

    
}