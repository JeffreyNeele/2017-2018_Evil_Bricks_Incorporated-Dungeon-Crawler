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
    int convIndex = 0;

    public Conversation(): base()
    {
        displayedText = new GameObjectList(1, "displayedtext");
        Add(displayedText);
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
        Position = new Vector2(0, 0);
        Add(conversationFrame);

        //Laadt het font in
        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext");
        currentText.Color = Color.White;
        currentText.Text = textLines[convIndex];
        currentText.Position = new Vector2(100, conversationFrame.Height / 2);
        displayedText.Add(currentText);
    }



    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }




    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.KeyPressed(Keys.Space))
        {
            if (convIndex < textLines.Count - 1)
            {
                convIndex += 1;
                displayedText.Clear();

                if (textLines[convIndex].StartsWith("#")) //een eerste teken # geeft aan dat het om een choice gaat hier. Daar zijn er altijd 3 van achter elkaar
                {
                    for (int i = 0; i < 3; i++)
                    {
                        TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext"); //maakt elke keer een nieuwe currentText aan zodat hij er meerdere weergeeft.
                        currentText.Position = new Vector2(100, i * 20 + 80); //voor y coordinaat: i*spacing + offset
                        currentText.Text = textLines[convIndex];
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
                    TextGameObject currentText = new TextGameObject("Assets/Fonts/ConversationFont", 0, "currentlydisplayedtext");
                    currentText.Text = textLines[convIndex];
                    currentText.Position = new Vector2(100, 114);
                    displayedText.Add(currentText);
                }
            }
        }
    }
}





