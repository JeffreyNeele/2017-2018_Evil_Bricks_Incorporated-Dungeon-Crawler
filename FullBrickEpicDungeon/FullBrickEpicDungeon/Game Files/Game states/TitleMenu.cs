using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TitleMenuState : IGameLoopObject
{
    Button startButton, settingsButton;
    Texture2D background;

    /// <summary>
    /// Class that defines the Title Menu of the game
    /// </summary>
    public TitleMenuState()
    {
        // load the background
        background = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Menu/LarrySketch");

        // load a settings and start button
        startButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        startButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - startButton.Width / 2), (GameEnvironment.Screen.Y * 3 / 4 - startButton.Height / 2));

        settingsButton = new Button("Assets/Sprites/Menu/SettingsButton");
        settingsButton.Position = new Vector2(GameEnvironment.Screen.X - settingsButton.Width, 0);
    }

    /// <summary>
    /// Draws the title menu
    /// </summary>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, Vector2.Zero, Color.White);
        startButton.Draw(gameTime, spriteBatch);
        settingsButton.Draw(gameTime, spriteBatch);
    }

    public void Update(GameTime gameTime)
    {

    }

    /// <summary>
    /// Handles the input for the title menu
    /// </summary>
    public void HandleInput(InputHelper inputHelper)
    {
        // Updates the input for the start and settingsbutton
        startButton.HandleInput(inputHelper);
        settingsButton.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
            //GameEnvironment.GameStateManager.SwitchTo("playingState");
            //GameEnvironment.GameStateManager.SwitchTo("characterSelection");
            GameEnvironment.GameStateManager.SwitchTo("cutscene");
        }
        if (settingsButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            GameEnvironment.GameStateManager.SwitchTo("settingsState");
        }
    }

    public void Setup() { }

    public void Reset()
    {
        startButton.Reset();
    }

    
}