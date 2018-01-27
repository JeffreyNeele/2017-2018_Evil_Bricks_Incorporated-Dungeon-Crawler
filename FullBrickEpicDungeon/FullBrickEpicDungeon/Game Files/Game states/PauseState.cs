using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

//State that pauses the game
class PauseState : MenuState
{
    protected IGameLoopObject playingState;
    protected Button continueButton, quitButton;
    protected Texture2D overlay;
    /// <summary>
    /// Class that defines a Pause state
    /// </summary>
    public PauseState() :base()
    {
        // find the playing state
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        overlay = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Paused/overlay");
    }

    protected override void FillButtonList()
    {
        // make buttons for the different assignments, eg return to menu
        continueButton = new Button("Assets/Sprites/Paused/Continue", 99);
        buttonList.Add(continueButton);

        quitButton = new Button("Assets/Sprites/Paused/ReturnToMenu", 99);
        buttonList.Add(quitButton);

        base.FillButtonList(); //gives positions to the buttons and the marker
    }

    public override void Update(GameTime gameTime)
    {
        //playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        //draw the overlay
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);

        base.Draw(gameTime, spriteBatch);
    }



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
                    case 0: //Continue button pressed
                        (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).Reset();
                        GameEnvironment.GameStateManager.SwitchTo("playingState");
                        break;
                    case 1: //Quit button pressed

                        FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
                        (GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState).ResetLevelIndex();
                        GameEnvironment.GameStateManager.SwitchTo("titleMenu");
                        break;
                    default: throw new IndexOutOfRangeException("Buttonbehaviour not defined. Buttonnumber in buttonList: " + buttonnr);

                }
            }
        }
    }
}

