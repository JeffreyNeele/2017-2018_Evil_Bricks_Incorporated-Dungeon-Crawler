using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Conversation : GameObjectList
{
    GameObjectList conversationField; //dit is een verzameling van alle gameobjects ed die worden weergegeven op het scherm.
    List<string> textLines; //alle regels uit de txt komen hierin


    TextGameObject currentText; //huidig weergegeven tekst
    List<string> currentChoices = new List<string>(); //dit is de list met de huidige choices pure strings.

    int convIndex = 0;

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
        conversationField = new GameObjectList(100, "conversation_total");
        Add(conversationField);

        //Laadt de sprite in van het frame eromheen
        SpriteGameObject conversationFrame = new SpriteGameObject("Assets/Sprites/Conversation Boxes/conversationbox1", 99, "", 0, false);
        conversationField.Position = new Vector2(0, 0);
        conversationField.Add(conversationFrame);

        //Laadt het font in
        currentText = new TextGameObject("Assets/Fonts/ConversationFont", 100);
        currentText.Color = Color.White;
        currentText.Text = textLines[convIndex];
        currentText.Position = new Vector2(100, conversationFrame.Height / 2);
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
                convIndex += 1;

            

            if (textLines[convIndex].StartsWith("#")) //een eerste teken # geeft aan dat het om een choice gaat hier. Daar zijn er altijd 3 van achter elkaar
            {
                TextGameObject choiceTekst1 = new TextGameObject("Assets/Fonts/ConversationFont", 100);
                TextGameObject choiceTekst2 = new TextGameObject("Assets/Fonts/ConversationFont", 100);
                TextGameObject choiceTekst3 = new TextGameObject("Assets/Fonts/ConversationFont", 100);
                choiceTekst1.Position = new Vector2(100, 80);
                choiceTekst2.Position = new Vector2(100, 100);
                choiceTekst3.Position = new Vector2(100, 120);

                    choiceTekst1.Text = textLines[convIndex];
                    choiceTekst2.Text = textLines[convIndex +1];
                    choiceTekst3.Text = textLines[convIndex +2];
                    convIndex += 2;
                    

                    
                conversationField.Add(choiceTekst1);
                conversationField.Add(choiceTekst2);
                conversationField.Add(choiceTekst3);

                
               // currentText.Visible = false; //hiermee maak ik alleen de laatste optie invisible
            }
            else
            {
                currentText.Text = textLines[convIndex];
                currentText.Position = new Vector2(100, 114);
                conversationField.Add(currentText);
            }


        }
    }
}





