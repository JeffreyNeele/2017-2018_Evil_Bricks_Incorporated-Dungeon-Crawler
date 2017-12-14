using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Conversation:GameObjectList
{
    GameObjectList conversationField; //dit is een verzameling van alle gameobjects ed die worden weergegeven op het scherm.
    List<string> textLines; //alle regels uit de txt komen hierin
    

    TextGameObject currentText; //huidig weergegeven tekst
    TextGameObject currentChoice; //als er keuzes op het scherm moeten verschijnen is dit het gameobject waarin de kleur en positie worden aangepast
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
            if(line != null)
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
        SpriteGameObject conversationFrame = new SpriteGameObject("Assets/Sprites/Conversation Boxes/conversationbox1", 99,"",0,false);
        conversationField.Position = new Vector2(0, 0);
        conversationField.Add(conversationFrame);

        //Laadt het font in
        currentText = new TextGameObject("Assets/Fonts/ConversationFont", 100);
        currentText.Color = Color.White;
        currentText.Text = textLines[convIndex];
        currentText.Position = new Vector2(100, conversationFrame.Height / 2);        
    }

    public void UpdateConversationText()
    {
        conversationField.Add(currentText);
    }

    public void UpdateToChoices()
    {
        for(int i = 0; i<3; i++) //voor alle 3 de choices, zet ze op de juiste positie en voeg ze toe aan het conversation field.
        {
            currentChoice = new TextGameObject("Assets/Fonts/ConversationFont", 100);
            currentChoice.Color = Color.White;
            currentChoice.Text = currentChoices[i];
            currentChoice.Position = new Vector2(100, i * 20 + 80); //voor y coordinaat: i*spacing + offset
            conversationField.Add(currentChoice);
        }

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

            currentText.Text = textLines[convIndex];

            if (textLines[convIndex].StartsWith("#")) //een eerste teken # geeft aan dat het om een choice gaat hier. Daar zijn er altijd 3 van achter elkaar
            {
                
               currentChoices.Add(currentText.Text); 

                if(currentChoices.Count == 3)
                {
                    UpdateToChoices();
                    currentChoices.Clear(); //cleart het voor de volgende ronde

                }
            }
            else
            {
                //Tenzij de convIndex bij de laatste ingelezen regel is gekomen, geeft hij de volgende regel tekst als er op spatie wordt gedrukt.
                UpdateConversationText();
            }


        }
    }


}

