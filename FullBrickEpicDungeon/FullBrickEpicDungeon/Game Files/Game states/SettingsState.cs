using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class SettingsState : IGameLoopObject
{
    private Texture2D settingsBackground;
    protected Button SFX, music, back;
    public SettingsState()
    {
        settingsBackground = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Settings/settingsbackground");
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

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(settingsBackground, Vector2.Zero, Color.White);
        SFX.Draw(gameTime, spriteBatch);
        music.Draw(gameTime, spriteBatch);
        back.Draw(gameTime, spriteBatch);
    }

    public void HandleInput(InputHelper inputHelper)
    {
        SFX.HandleInput(inputHelper);
        music.HandleInput(inputHelper);
        back.HandleInput(inputHelper);
        if (back.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
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

    public void Reset()
    {
        SFX.Reset();
        music.Reset();
        back.Reset();
    }
}