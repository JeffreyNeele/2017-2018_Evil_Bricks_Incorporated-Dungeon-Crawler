using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

class TitleMenuState : MenuState
{
    Button startButton, settingsButton, quitButton;
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
        startButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        buttonList.Add(startButton);
        //startButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - startButton.Width / 2), (GameEnvironment.Screen.Y * 3 / 4 - startButton.Height / 2));

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(settingsButton);
        //settingsButton.Position = new Vector2(GameEnvironment.Screen.X - settingsButton.Width, 0);

        quitButton = new Button("Assets/Sprites/Menu/SettingsButton");
        buttonList.Add(quitButton);

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
    //update and handleinput deleted

  


    protected override void HandleMouseInput(InputHelper inputHelper)
    {
        // Updates the input for the start and settingsbutton
        startButton.HandleInput(inputHelper);
        settingsButton.HandleInput(inputHelper);
        //Pressed automatically becomes true when someone clicks the button.

        base.HandleMouseInput(inputHelper);
    }

    protected override void PressButton()
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

    protected override void ButtonPressedHandler()
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

    public override void Reset()
    {
        startButton.Reset();
    }

    
}