using Microsoft.Xna.Framework;

class TitleMenuState: GameObjectList
    {
    protected Button playButton, loadButton, settingsButton, quitButton;

    public TitleMenuState()
    {
        // load the title screen
        SpriteGameObject titleScreen = new SpriteGameObject("Backgrounds/spr_title", 0, "background");
        Add(titleScreen);

        // add a play button
        playButton = new Button("Sprites/spr_button_play", 1);
        playButton.Position = new Vector2((GameEnvironment.Screen.X - playButton.Width) / 2, 540);
        Add(playButton);

        // add a help button
        loadButton = new Button("Sprites/spr_button_load", 1);
        loadButton.Position = new Vector2((GameEnvironment.Screen.X - loadButton.Width) / 2, 600);
        Add(loadButton);

        settingsButton = new Button("Sprites/spr_button_settings", 1);
        settingsButton.Position = new Vector2((GameEnvironment.Screen.X - settingsButton.Width / 2), 640);
        Add(settingsButton);

        quitButton = new Button("Sprites/spr_button_quit", 1);
        quitButton.Position = new Vector2((GameEnvironment.Screen.X - quitButton.Width / 2), 700);
        Add(quitButton);

    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (playButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("characterSelection");
        }
        else if (loadButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("loading");
        }
        else if (settingsButton.Pressed)
        {
            GameEnvironment.GameStateManager.SwitchTo("settings");
        }
        else if(quitButton.Pressed)
        {
           //TO DO
        }
    }
}

