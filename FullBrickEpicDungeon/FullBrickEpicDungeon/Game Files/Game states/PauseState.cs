using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//State that pauses the game
class PauseState : IGameLoopObject
{
    protected IGameLoopObject playingState;
    protected List<Button> buttonList = new List<Button>();
    protected Button continueButton, quitButton;
    protected Texture2D overlay;
    protected SpriteGameObject marker;

    protected Vector2 offsetmarker;
    protected int buttonSeparation = 250;

    //toetsenbord controls dictionary van player 0 en 1 in de dictionary hiervoor
    protected Dictionary<Keys, Keys> keyboardControls1;
    protected Dictionary<Keys, Keys> keyboardControls2;

    //matches the player number to the controls dictionary used.
    protected Dictionary<int, Dictionary<Keys, Keys>> keyboardcontrols = new Dictionary<int, Dictionary<Keys, Keys>>();

   


    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");
        marker = new SpriteGameObject("Assets/Sprites/Conversation Boxes/arrow", 1, "", 10, false);

        keyboardControls1 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        keyboardControls2 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        keyboardcontrols.Add(0, keyboardControls1);
        keyboardcontrols.Add(1, keyboardControls2);

        FillButtonList();


    }

    private void FillButtonList()
    {
        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue", 99);
        buttonList.Add(continueButton);

        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu", 99);
        buttonList.Add(quitButton);

        //set button positions
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - continueButton.Width / 2, 250 + i * buttonSeparation);
        }

        marker.Position = new Vector2(continueButton.Position.X - offsetmarker.X, continueButton.Position.Y - offsetmarker.Y);
        offsetmarker = new Vector2(marker.Width, 5 + continueButton.Height / 2);
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
        foreach (Button button in buttonList)
        {
            button.Draw(gameTime, spriteBatch);
        }
        
        // draw the marker
        marker.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Handles input for the pause state
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
            if (marker.Position.Y < buttonList[buttonList.Count - 1].Position.Y + offsetmarker.Y)
            {
                marker.Position += new Vector2(0, buttonSeparation);
            }

        }
        if (inputHelper.ButtonPressed(controllernumber, Buttons.DPadUp) || inputHelper.MenuDirection(controllernumber, false, true).Y > 0)
        {
            if (marker.Position.Y > buttonList[buttonList.Count - 1].Position.Y + offsetmarker.Y)
            {
                marker.Position -= new Vector2(0, buttonSeparation);
            }
        }
    }

    private void HandleKeyboardInput(InputHelper inputHelper)
    {
        //moves the marker up or down depending on the key that was pressed, the Dpad or the Thumbstick.
        if (inputHelper.KeyPressed(Keys.Down))
        {
            if (marker.Position.Y < buttonList[buttonList.Count - 2].Position.Y + offsetmarker.Y) //waarom -2? Omdat -1 niet werkte
            {
                marker.Position += new Vector2(0, buttonSeparation);
            }

        }
        if (inputHelper.KeyPressed(Keys.Up))
        {
            if (marker.Position.Y > buttonList[buttonList.Count - 1].Position.Y + offsetmarker.Y)
            {
                marker.Position -= new Vector2(0, buttonSeparation);
            }
        }
    }


    private void HandleMouseInput(InputHelper inputHelper)
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

