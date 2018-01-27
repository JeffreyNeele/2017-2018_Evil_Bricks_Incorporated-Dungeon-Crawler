using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

abstract class MenuState : IGameLoopObject
    {

    protected List<Button> buttonList = new List<Button>();

    protected SpriteGameObject marker;
    protected Vector2 offsetMarker;
    protected int buttonSeparation = 250;

    //toetsenbord controls dictionary van player 0 en 1 in de dictionary hiervoor
    protected Dictionary<Keys, Keys> keyboardControls1;
    protected Dictionary<Keys, Keys> keyboardControls2;

    //matches the player number to the controls dictionary used.
    protected Dictionary<int, Dictionary<Keys, Keys>> keyboardcontrols = new Dictionary<int, Dictionary<Keys, Keys>>();




    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public MenuState()
    {        
        marker = new SpriteGameObject("Assets/Sprites/Conversation Boxes/arrow", 1, "", 10, false);

        keyboardControls1 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player1controls.txt");
        keyboardControls2 = GameEnvironment.SettingsHelper.GenerateKeyboardControls("Assets/Controls/player2controls.txt");
        keyboardcontrols.Add(0, keyboardControls1);
        keyboardcontrols.Add(1, keyboardControls2);

        FillButtonList();


    }

    protected virtual void FillButtonList()
    {
       
        //set button positions
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Position = new Vector2(GameEnvironment.Screen.X / 2 - buttonList[0].Width / 2, 250 + i * buttonSeparation);
        }
        offsetMarker = new Vector2(-marker.Width, buttonList[0].Height / 2 - marker.Height / 2);
        marker.Position = new Vector2(buttonList[0].Position.X - offsetMarker.X, buttonList[0].Position.Y - offsetMarker.Y);
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
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
    public virtual void HandleInput(InputHelper inputHelper)
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

    protected virtual void HandleXboxInput(InputHelper inputHelper, int controllernumber)
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
        if (inputHelper.ButtonPressed(controllernumber, Buttons.A))
        {
            PressButton();
        }
    }

    protected virtual void HandleKeyboardInput(InputHelper inputHelper)
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


    protected virtual void HandleMouseInput(InputHelper inputHelper)
    {
        ButtonPressedHandler();
    }

    protected virtual void PressButton()
    {
        //sets the button.Pressed true or false behaviour. buttons differs Per menu
    }

    protected virtual void ButtonPressedHandler()
    {
       //sets button excecution. differs per menu
    }

    public virtual void Initialize() { }

    public virtual void Reset()
    {
    }

}




