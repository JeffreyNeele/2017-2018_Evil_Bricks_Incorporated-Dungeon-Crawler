using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

class GameOverState : IGameLoopObject
{
    protected IGameLoopObject playingState;
    Button quitButton;
    Texture2D overlay, plaque;
    public GameOverState()
    {
        overlay = GameEnvironment.AssetManager.GetSprite("");
        plaque =  GameEnvironment.AssetManager.GetSprite("");
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu");
        quitButton.Position = new Vector2(GameEnvironment.Screen.X / 2 - quitButton.Width / 2, 500);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
        spriteBatch.Draw(plaque, Vector2.Zero, Color.White);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        quitButton.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.E) || inputHelper.AnyPlayerPressed(Buttons.Y))
        {
            playingState.Reset();
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        if (quitButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
    }

    public void Initialize()
    { }

    public void Reset()
    {
        quitButton.Reset();
    }

    public void Update(GameTime gameTime)
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }
}

