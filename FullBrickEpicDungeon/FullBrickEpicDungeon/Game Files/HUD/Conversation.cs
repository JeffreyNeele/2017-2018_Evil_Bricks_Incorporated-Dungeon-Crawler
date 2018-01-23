using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Conversation : GameObjectList
{
    List<string> textLines; //all the lines from the text file are added to this list
    GameObjectList displayedText; // Gameobjectlist for the currently displayed text
    List<string> currentChoices = new List<string>(); // list for the currently displayed choices
    int convIndex = 0, compensation = 0; //convIndex is regel in het bestand
    //compensation is als keuze is geweest, de extra regels die hij moet overslaan na weergegeven antwoord.
    SpriteGameObject marker = new SpriteGameObject("Assets/Sprites/Conversation Boxes/arrow", 1, "", 10, false);

    bool PreviousLineWasChoice = false;

    Vector2 upperChoicePos;
    const int choiceSeperation = 35;
    float bottomChoiceHeight;
    int offsetMarker;

    /// <summary>
    /// Class that makes a Conversation box with possible choices / story
    /// </summary>
    /// <param name="path">the path the Conversation file is at</param>
    public Conversation(string path)
    {
        upperChoicePos = new Vector2(25, 60);
        bottomChoiceHeight = upperChoicePos.Y + 2 * choiceSeperation;

        displayedText = new GameObjectList(1, "displayedtext");
        Add(displayedText);
        LoadConversation(path);
        ShowConversationBox();
        Add(marker);
        offsetMarker = -5;

        marker.Position = new Vector2(upperChoicePos.X, upperChoicePos.Y + offsetMarker);
        marker.Visible = false;   
    }

    /// <summary>
    /// Loads a Conversation from a path
    /// </summary>
    /// <param name="path">the path the .txt file that needs to be loaded is found at</param>
    public void LoadConversation(string path)
    {
        path = "Content/" + path;
        textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }
    }




    /// <summary>
    /// Loads the conversation box for the first time (with the first text)
    /// </summary>
    public void ShowConversationBox()
    {
        SpriteGameObject conversationFrame = new SpriteGameObject("Assets/Sprites/Conversation Boxes/conversationbox1", 0, "", 10, false);
        Position = new Vector2(GameEnvironment.Screen.X / 2 - conversationFrame.Width / 2, GameEnvironment.Screen.Y * 3 / 4);
        Add(conversationFrame);
        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext")
        {
            Color = Color.White,
            Text = textLines[convIndex],
            Position = new Vector2(100, conversationFrame.Height / 2 - 20)
        };
        displayedText.Add(currentText);
    }



    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    /// <summary>
    /// Handles the input for the conversation box
    /// </summary>
    /// <param name="inputHelper">input helper</param>
    public override void HandleInput(InputHelper inputHelper)
    {

        base.HandleInput(inputHelper);
        // Moves the conversation index forward appropiately 
        if (inputHelper.KeyPressed(Keys.Space) || inputHelper.ButtonPressed(1, Buttons.A))
        {
            if (convIndex < textLines.Count - 1)
            {
                if (PreviousLineWasChoice)
                {

                    if (marker.Position.Y == upperChoicePos.Y + offsetMarker)
                    {
                        convIndex++;
                        compensation = 2;
                    }
                    else if (marker.Position.Y == upperChoicePos.Y + offsetMarker + choiceSeperation && convIndex < textLines.Count - 2)
                    {
                        convIndex += 2;
                        compensation = 1;
                    }
                    else if (marker.Position.Y == bottomChoiceHeight + offsetMarker && convIndex < textLines.Count - 3)
                    {
                        convIndex += 3;
                    }
                }
                else if (!PreviousLineWasChoice)
                {
                    convIndex++;
                }
                displayedText.Clear();
                PreviousLineWasChoice = false;




                if (textLines[convIndex].StartsWith("#")) //a # signifies that there is a choice in the text file
                {

                    marker.Visible = true;
                    PreviousLineWasChoice = true;
                    for (int i = 0; i < 3; i++)
                    {
                        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext")
                        {
                            Position = new Vector2(100, i * choiceSeperation + upperChoicePos.Y),
                            Text = textLines[convIndex]
                        };
                        displayedText.Add(currentText);

                        if (convIndex < textLines.Count - 1 && i < 2)
                        {
                            convIndex += 1;
                        }
                    }
                }
                else
                {
                    marker.Visible = false;
                    TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext")
                    {
                        Text = textLines[convIndex],
                        Position = new Vector2(100, conversationFrame.Height / 2 - 20)
                    };
                    displayedText.Add(currentText);
                }
                if (compensation > 0 && convIndex < textLines.Count - compensation)
                {
                    convIndex += compensation;
                    compensation = 0;
                }

            }
            else
            {
                convIndex = 0;
                // stops the conversation box from displaying itself and switches back to the playing state
                (GameEnvironment.GameStateManager.CurrentGameState as ConversationState).GoToNextConversation();
            }
        }

        // Moves the marker up or down depending on the key that was pressed
        if (inputHelper.KeyPressed(Keys.Down) || inputHelper.ButtonPressed(1, Buttons.DPadDown))
        {
            if (marker.Position.Y < bottomChoiceHeight + offsetMarker)
            {
                marker.Position += new Vector2(0, choiceSeperation);
            }

        }
        if (inputHelper.KeyPressed(Keys.Up) || inputHelper.ButtonPressed(1, Buttons.DPadUp))
        {
            if (marker.Position.Y > upperChoicePos.Y + offsetMarker)
            {
                marker.Position -= new Vector2(0, choiceSeperation);
            }
        }
    }

   /* public override void Reset()
    {
        PreviousLineWasChoice = false;
        base.Reset();
    }*/
}







