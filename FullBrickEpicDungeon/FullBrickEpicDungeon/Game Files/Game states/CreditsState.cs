using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

class CreditsState : MenuState
{
    private Texture2D settingsBackground, creditsBackground;
    protected Button back;
    bool prevPause;
    /// <summary>
    /// Class that displays a settings screen.
    /// </summary>
    public CreditsState() : base()
    {
        // Load the background
        creditsBackground = GameEnvironment.AssetManager.GetSprite("Assets/Credits/Achtergrond");
        // vervangen door een goeie achtergrond !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11
    }




    protected override void FillButtonList()
    {
        // Load the button the back button.
        back = new Button("Assets/Sprites/Settings/ReturnToMenu");
            buttonList.Add(back);
        //set button positions 
        back.Position = new Vector2(GameEnvironment.Screen.X / 2 - back.Width / 2, 850);
        // align the marker
        offsetMarker = new Vector2(-marker.Width, back.Height / 2 - marker.Height / 2);
        marker.Position = new Vector2(back.Position.X + offsetMarker.X, back.Position.Y + offsetMarker.Y);


    }

    protected override void HandleXboxInput(InputHelper inputHelper, int controllernumber)
    {

        if (inputHelper.ButtonPressed(controllernumber, Buttons.B))
        {
            buttonList[2].Pressed = true; //Back to main menu if B is pressed.
            ButtonPressedHandler();
        }
        base.HandleXboxInput(inputHelper, controllernumber);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // vervangen door een goeie achtergrond met credits ipv settings. !!!!!
        spriteBatch.Draw(creditsBackground, Vector2.Zero, Color.White);
        base.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// if the button is pressed (which is handled in MenuState) this method executes what happens
    /// </summary>
    protected override void ButtonPressedHandler()
    {
        for (int buttonnr = 0; buttonnr < buttonList.Count; buttonnr++)
        {
            if (buttonList[buttonnr].Pressed)
            {
                switch (buttonnr)
                {
                    case 0: //Back
                        if (back.Pressed)
                        {
                            if (prevPause)
                                GameEnvironment.GameStateManager.SwitchTo("pauseState");
                            if (!prevPause)
                                GameEnvironment.GameStateManager.SwitchTo("titleMenu");
                        }
                        break;
                    default: throw new IndexOutOfRangeException("Buttonbehaviour not defined. Buttonnumber in buttonList: " + buttonnr);
                }
                GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            }
        }
    }
    public override void Initialize()
    {
        IGameLoopObject prevGameState = GameEnvironment.GameStateManager.PreviousGameState as IGameLoopObject;
        // draw over the playingstate if it was in playingstate
        if (prevGameState == GameEnvironment.GameStateManager.GetGameState("pauseState"))
        {
            prevPause = true;
        }
        //draw over cutscene if previousstate was cutscene
        else if (prevGameState == GameEnvironment.GameStateManager.GetGameState("titleMenu"))
        {
            prevPause = false;
        }
        else
        {
            throw new Exception("Cutscene cannot be called from this GameState" + GameEnvironment.GameStateManager.PreviousGameState);
        }
    }

}