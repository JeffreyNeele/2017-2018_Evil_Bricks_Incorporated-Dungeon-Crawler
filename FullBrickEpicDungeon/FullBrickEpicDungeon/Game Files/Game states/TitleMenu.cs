using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

class TitleMenuState : MenuState
{
    Button startButton, settingsButton;
    Texture2D background;


    /// <summary>
    /// Class that defines the Title Menu of the game
    /// </summary>
    public TitleMenuState() : base()
    {
        // load the background
        background = GameEnvironment.AssetManager.GetSprite("Assets/Cutscenes/LarryShits");
    }


    protected override void FillButtonList()
    {
        // load a settings and start button
        startButton = new Button("Assets/Sprites/Menu/StartButton");
        buttonList.Add(startButton);

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(settingsButton);

        base.FillButtonList();
    }




    /// <summary>
    /// Draws the title menu
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, Vector2.Zero, Color.White);

        base.Draw(gameTime, spriteBatch);
    }


    /// <summary>
    /// if the button is pressed (which is handled in MenuState) this method executes what happens
    /// </summary>
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
                    case 0: //Start button pressed
                        FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
                        GameEnvironment.GameStateManager.SwitchTo("characterSelection");
                        break;
                    case 1: //Settings button pressed
                        GameEnvironment.GameStateManager.SwitchTo("settingsState");
                        break;
                    default: throw new IndexOutOfRangeException("Buttonbehaviour not defined. Buttonnumber in buttonList: " + buttonnr);

                }
            }
        }
    }
}