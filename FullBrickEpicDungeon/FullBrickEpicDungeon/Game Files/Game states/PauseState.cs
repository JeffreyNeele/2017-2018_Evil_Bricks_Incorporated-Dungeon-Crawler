using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // we draw the playingstate but we do not update it because we want the pause state to be an overlay.
        playingState.Draw(gameTime, spriteBatch);
        //draw the overlay
        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);

        base.Draw(gameTime, spriteBatch);
    }


    private void HandleMouseInput(InputHelper inputHelper)
    {
        continueButton.HandleInput(inputHelper);
        quitButton.HandleInput(inputHelper);
        if (continueButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = false;
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }

        else if (quitButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            playingState.Reset();
            FullBrickEpicDungeon.DungeonCrawler.mouseVisible = true;
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }
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
                        continueButton.Pressed = true;
                        break;
                    case 1:
                        quitButton.Pressed = true;
                        break;
                    //Pressed wordt automatisch weer op false gezet door de ButtonInputHandler van de muis.
                    default: throw new System.Exception("Button behaviour not specified. Number in buttonList: " + index);
                }

                ButtonPressedHandler();
            }
        }
    }

    public void Initialize() { }

    public void Reset()
    {
        continueButton.Reset();
        quitButton.Reset();
    }
}

