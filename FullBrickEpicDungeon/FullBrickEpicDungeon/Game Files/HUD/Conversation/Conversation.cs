using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework;


class Conversation:GameObjectList
{
    List<string> textLines;



    //Laadt de conversatie in uit een text bestand als LoadConversation aangeroepen wordt op locatie path. Deze komt in een List te staan.
    //Kan als Load Level af is daar ook in worden gezet. In de input wordt de eerste en laatste line aangegeven die uitgelezen moet worden.
    //lastLine staat standaard op de maximale value die de int kan hebben, dus meer dan genoeg regels voor elk txt bestand.
    public void LoadConversation(string path, int startingLine = 0, int lastLine = Int32.MaxValue)
    {
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
    }




    //Laat als aangeroepen de sprite met text zien op het scherm met momenteel de eerste regel van de test txt file.
    public void ShowConversation()
    {
        GameObjectList conversationField = new GameObjectList(100, "conversation_total");
        Add(conversationField);

        //Laadt de sprite in van het frame eromheen
        SpriteGameObject conversationFrame = new SpriteGameObject("Conversations/ConversationBox3", 99,"",0,false);
        conversationField.Position = new Vector2(0, 0);
        conversationField.Add(conversationFrame);

        //Laadt het font in
        TextGameObject currentText = new TextGameObject("Fonts/ConversationFont", 100);
        currentText.Text = textLines[1];
        currentText.Position = new Vector2(100, conversationFrame.Height / 2);
        currentText.Color = Color.White;
        conversationField.Add(currentText);
        conversationField.Visible = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
