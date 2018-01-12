 using Microsoft.Xna.Framework;

class TitleMenuState : GameObjectList
{
    // protected Button playButton, loadButton, settingsButton, quitButton;
    protected Button startButton;

    public TitleMenuState()
    {

        // load the title screen
        SpriteGameObject titleScreen = new SpriteGameObject("Assets/Sprites/Menu/LarrySketch", 0, "background");
        Add(titleScreen);

        // add a play button
        startButton = new Button("Assets/Sprites/Menu/StartButton", 1);
        startButton.Position = new Vector2((GameEnvironment.Screen.X / 2 - startButton.Width / 2), (GameEnvironment.Screen.Y * 3 / 4 - startButton.Height / 2));
        Add(startButton);

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (startButton.Pressed)
        {
            GameEnvironment.AssetManager.PlaySound("Assets/SFX/button_click");
            GameEnvironment.GameStateManager.SwitchTo("playingState");
            //GameEnvironment.GameStateManager.SwitchTo("characterSelection");
        }
    }

}