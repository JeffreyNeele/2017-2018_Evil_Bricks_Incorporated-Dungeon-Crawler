using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System;

class SettingsState : MenuState
{
    private Texture2D settingsBackground;
    protected Button SFX, music, back;
    bool prevPause;
    /// <summary>
    /// Class that displays a settings screen.
    /// </summary>
    public SettingsState() : base()
    {
        // Load the background
        settingsBackground = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Settings/settingsbackground");
       
    }




    protected override void FillButtonList()
    {
        // Load the buttons for the SFX toggler, music toggler and the back button.
        SFX = new Button("Assets/Sprites/Settings/sfxbutton@2");
        buttonList.Add(SFX);
        music = new Button("Assets/Sprites/Settings/musicbutton@2");
        buttonList.Add(music);
        back = new Button("Assets/Sprites/Settings/ReturnToMenu");
        buttonList.Add(back);

        base.FillButtonList();
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
        spriteBatch.Draw(settingsBackground, Vector2.Zero, Color.White);
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
                    case 0: //SFX
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
                        break;
                    case 1: //Music
                        if (music.Sprite.SheetIndex == 0)
                        {
                            FullBrickEpicDungeon.DungeonCrawler.music = false;
                            music.Sprite.SheetIndex = 1;
                            MediaPlayer.Stop();
                        }
                        else
                        {
                            FullBrickEpicDungeon.DungeonCrawler.music = true;
                            music.Sprite.SheetIndex = 0;
                            if(MediaPlayer.State == MediaState.Stopped)
                                GameEnvironment.AssetManager.PlayMusic("Assets/Music/menu");
                        }
                        break;
                    case 2: //Back
                        if (back.Pressed)
                        {
                            if(prevPause)
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