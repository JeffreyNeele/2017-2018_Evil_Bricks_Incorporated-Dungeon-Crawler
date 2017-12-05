using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;


class Conversation:GameObjectList
{
    List<string> textLines;
    //Laadt de conversatie in uit een text bestand als LoadConversation aangeroepen wordt op locatie path. Deze komt in een List te staan.
    //Kan als Load Level af is daar ook in worden gezet.

    public void LoadConversation(string path)
    {
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

    public void ShowConversation()
    {
        GameObjectList conversationField = new GameObjectList(100, "conversation_total");
        Add(conversationField);

       // string conversationsprite = Path.GetFullPath("Conversations/ConversationBox3.png");
        //Laadt de sprite in van het frame eromheen
        SpriteGameObject hintFrame = new SpriteGameObject("Conversations/ConversationBox3", 99);
        conversationField.Position = new Vector2((GameEnvironment.Screen.X - hintFrame.Width) / 2, 10);
        conversationField.Add(hintFrame);

        //Laadt het font in
        TextGameObject currentText = new TextGameObject("Fonts/ConversationFont", 100);
        currentText.Text = textLines[0];
        currentText.Position = new Vector2(120, 25);
        currentText.Color = Color.Black;
        conversationField.Add(currentText);
        currentText.Visible = true;
    }
}
