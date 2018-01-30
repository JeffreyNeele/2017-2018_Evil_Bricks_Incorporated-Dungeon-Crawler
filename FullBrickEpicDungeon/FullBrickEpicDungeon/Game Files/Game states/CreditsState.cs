using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

class CreditsState : MenuState
{
    private Texture2D creditsBackground;
    protected Button back;
    bool prevPause;
    SpriteGameObject conversationFrame;
    GameObjectList credits;


    /// <summary>
    /// Class that displays a settings screen.
    /// </summary>
    public CreditsState() : base()
    {
        // Load the background
        creditsBackground = GameEnvironment.AssetManager.GetSprite("Assets/Credits/Achtergrond");
        credits = new GameObjectList(100);
        LoadCredits("Assets/Credits/CreditText.txt");
    }




    protected override void FillButtonList()
    {
        // Load the button the back button.
        back = new Button("Assets/Sprites/Settings/ReturnToMenu");
            buttonList.Add(back);
        //set button positions 
        back.Position = new Vector2(GameEnvironment.Screen.X / 2 - back.Width / 2, 850);
        // align the marker
        offsetMarker = new Vector2(-marker.Width, back.Height / 2 - marker.Height / 2);
        marker.Position = new Vector2(back.Position.X + offsetMarker.X, back.Position.Y + offsetMarker.Y);


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
        spriteBatch.Draw(creditsBackground, Vector2.Zero, Color.White);
        credits.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        credits.Update(gameTime);
        base.Update(gameTime);
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
                    case 0: //Back
                        if (back.Pressed)
                        {
                            if (prevPause)
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

    }


    public void LoadCredits(string path)
    {  // leest de tekst uit de .txt file uit
        path = "Content/" + path;
        List<string> textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }
        ShowCredits(textLines);
    }

       // tekent de tekst op het scherm
    private void ShowCredits(List<string> creditLines)
    {
        for (int i = 0; i < creditLines.Count; i++)
        {
            TextGameObject creditline = new TextGameObject("Assets/Fonts/ConversationFont")
            {
                Color = Color.Red,
                Text = creditLines[i],
            };
            creditline.Position = new Vector2(GameEnvironment.Screen.X / 2 - creditline.Size.X / 2, 200 + i * 50);
            credits.Add(creditline);
        }
    }
}