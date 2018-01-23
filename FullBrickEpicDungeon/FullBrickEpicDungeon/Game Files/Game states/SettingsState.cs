using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class SettingsState : IGameLoopObject
{
    private Texture2D settingsBackground;
    protected Button SFX, music, back;
    /// <summary>
    /// Class that displays a settings screen.
    /// </summary>
    public SettingsState()
    {
        // Load the background
        settingsBackground = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Settings/settingsbackground");
        // Load the buttons for the SFX toggler, music toggler and the back button.
        SFX = new Button("Assets/Sprites/Settings/sfxbutton@2");
        SFX.Position = new Vector2(GameEnvironment.Screen.X / 2 - SFX.Width / 2, 250);
        music = new Button("Assets/Sprites/Settings/musicbutton@2");
        music.Position = new Vector2(GameEnvironment.Screen.X / 2 - music.Width / 2, 400);
        back = new Button("Assets/Sprites/Settings/ReturnToMenu");
        back.Position = new Vector2(GameEnvironment.Screen.X / 2 - back.Width / 2, GameEnvironment.Screen.Y - (back.Height + 100));
    }

    public void Update(GameTime gameTime)
    {

    }
    /// <summary>
    /// Draws the settings menu
    /// </summary>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(settingsBackground, Vector2.Zero, Color.White);
        SFX.Draw(gameTime, spriteBatch);
        music.Draw(gameTime, spriteBatch);
        back.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Handles input for the settings menu
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        // handle input for the buttons
        SFX.HandleInput(inputHelper);
        music.HandleInput(inputHelper);
        back.HandleInput(inputHelper);
        // If the back button is pressed return to the title menu
        if (back.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
        // if the SFX button is pressed, switch to the opposite sprite and set the correct global SFX variable
        if (SFX.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            if (SFX.Sprite.SheetIndex == 0)
            {
                FullBrickEpicDungeon.DungeonCrawler.SFX = false;
                SFX.Sprite.SheetIndex = 1;
            }
            else
            {
                FullBrickEpicDungeon.DungeonCrawler.SFX = true;
                SFX.Sprite.SheetIndex = 0;
            }
               
        }
        // same thing for the music button
        if (music.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            if (music.Sprite.SheetIndex == 0)
            {
                FullBrickEpicDungeon.DungeonCrawler.music = false;
                music.Sprite.SheetIndex = 1;
            }
            else
            {
                FullBrickEpicDungeon.DungeonCrawler.music = true;
                music.Sprite.SheetIndex = 0;
            }
        }
    }

    public void Setup() { }

    /// <summary>
    /// Resets thed settings menu
    /// </summary>
    public void Reset()
    {
        SFX.Reset();
        music.Reset();
        back.Reset();
    }
}