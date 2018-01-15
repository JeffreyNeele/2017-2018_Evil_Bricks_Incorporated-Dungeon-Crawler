using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Conversation : GameObjectList
{
    List<string> textLines; //alle regels uit de txt komen hierin
    GameObjectList displayedText; //huidig weergegeven tekst
    List<string> currentChoices = new List<string>(); //dit is de list met de huidige choices pure strings.
    int convIndex = 0, compensation = 0;
    SpriteGameObject marker = new SpriteGameObject("Assets/Sprites/Conversation Boxes/arrow", 1, "", 10, false);
    bool PreviousLineWasChoice = false;
    Vector2 markerpos = new Vector2(25, 60);

    public Conversation(string path, int startingLine, int lastLine = Int32.MaxValue) : base()
    {
        displayedText = new GameObjectList(1, "displayedtext");
        Add(displayedText);
        LoadConversation(path, startingLine, lastLine);
        ShowConversationBox();
        Add(marker);
        marker.Position = markerpos;
        marker.Visible = false;
    }
    //Laadt de conversatie in uit een text bestand als LoadConversation aangeroepen wordt op locatie path. Deze komt in een List te staan.
    //Kan als Load Level af is daar ook in worden gezet. In de input wordt de eerste en laatste line aangegeven die uitgelezen moet worden.
    //lastLine staat standaard op de maximale value die de int kan hebben, dus meer dan genoeg regels voor elk txt bestand.
    public void LoadConversation(string path, int startingLine = 0, int lastLine = Int32.MaxValue)
    {
        path = "Content/" + path;
        textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        int width = line.Length;
        for (int l = startingLine; l <= lastLine; l++)
        {
            if (line != null)
            {
                textLines.Add(line);
                line = fileReader.ReadLine();
            }
        }
        // lineChars = line.ToCharArray();
    }




    //Laat als aangeroepen de sprite met text zien op het scherm met momenteel de eerste regel van de test txt file.
    public void ShowConversationBox()
    {

        //Laadt de sprite in van het frame eromheen
        SpriteGameObject conversationFrame = new SpriteGameObject("Assets/Sprites/Conversation Boxes/conversationbox1", 0, "", 10, false);
        Position = new Vector2(GameEnvironment.Screen.X / 2 - conversationFrame.Width / 2, GameEnvironment.Screen.Y * 3 / 4);
        Add(conversationFrame);

        //Laadt het font in
        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext")
        {
            Color = Color.White,
            Text = textLines[convIndex],
            Position = new Vector2(100, conversationFrame.Height / 2)
        };
        displayedText.Add(currentText);
    }



    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void HandleInput(InputHelper inputHelper)
    {

        base.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.Space) || inputHelper.ButtonPressed(1, Buttons.A))
        {
            if (convIndex < textLines.Count - 1)
            {
                if (PreviousLineWasChoice == false)
                {
                    convIndex++;
                }
                if (PreviousLineWasChoice == true && marker.Position.Y == markerpos.Y)
                {
                    convIndex++;
                    compensation = 2;
                }
                if (PreviousLineWasChoice == true && marker.Position.Y == markerpos.Y+20 && convIndex < textLines.Count - 2)
                {
                    convIndex += 2;
                    compensation = 1;
                }
                if (PreviousLineWasChoice == true && marker.Position.Y == markerpos.Y+20*2 && convIndex < textLines.Count - 3)
                {
                    convIndex += 3;
                }
                displayedText.Clear();
                PreviousLineWasChoice = false;


                if (textLines[convIndex].StartsWith("#")) //een eerste teken # geeft aan dat het om een choice gaat hier. Daar zijn er altijd 3 van achter elkaar
                {
                    
                    marker.Visible = true;
                    PreviousLineWasChoice = true;
                    for (int i = 0; i < 3; i++)
                    {
                        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext")
                        {
                            Position = new Vector2(100, i * 20 + 80), //voor y coordinaat: i*spacing + offset
                            Text = textLines[convIndex]
                        }; 
                        //maakt elke keer een nieuwe currentText aan zodat hij er meerdere weergeeft.
                        displayedText.Add(currentText);

                        if (convIndex < textLines.Count - 1 && i < 2)
                        {
                            convIndex += 1;
                        }
                    }

                    // currentText.Visible = false; //hiermee maak ik alleen de laatste optie invisible
                }
                else
                {
                    marker.Visible = false;
                    TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext");
                    currentText.Text = textLines[convIndex];
                    currentText.Position = new Vector2(100, 114);
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
                //ends the conversation box
                convIndex = 0;
                GameEnvironment.GameStateManager.SwitchTo("playingState");
            }
        }

        if ((inputHelper.KeyPressed(Keys.Down) || inputHelper.ButtonPressed(1, Buttons.DPadDown)) && marker.Position.Y < markerpos.Y + 2*20)
        {
            marker.Position += new Vector2(0, 20);

        }
        if ((inputHelper.KeyPressed(Keys.Up) || inputHelper.ButtonPressed(1, Buttons.DPadUp)) && marker.Position.Y > markerpos.Y)
        {
            marker.Position -= new Vector2(0, 20);
        }
    }
}







