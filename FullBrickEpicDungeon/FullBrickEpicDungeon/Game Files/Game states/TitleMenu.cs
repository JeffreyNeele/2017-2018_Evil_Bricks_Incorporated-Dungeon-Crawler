using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class TitleMenuState : IGameLoopObject
{
    // protected Button playButton, loadButton, settingsButton, quitButton;
    Button startButton;
    Texture2D background;

    public TitleMenuState()
    {
        background = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Menu/LarrySketch");

        // add a play button
        startButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        startButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - startButton.Width / 2), (GameEnvironment.Screen.Y * 3 / 4 - startButton.Height / 2));

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, Vector2.Zero, Color.White);
        startButton.Draw(gameTime, spriteBatch);
    }

    public void Update(GameTime gameTime)
    {

    }

    public void HandleInput(InputHelper inputHelper)
    {
        startButton.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
            GameEnvironment.GameStateManager.SwitchTo("playingState");
            //GameEnvironment.GameStateManager.SwitchTo("characterSelection");
        }
    }

    public void Reset()
    {

    }

    
}