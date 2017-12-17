using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Input;

class SettingsState : GameObjectList
{
    // protected Button playButton, loadButton, settingsButton, quitButton;

    public SettingsState()
    {
    }

    protected Dictionary<Keys, Keys> GenerateKeyboardControls(string path)
    {
        path = "Content/" + path;
        StreamReader fileReader = new StreamReader("Content/Assets/KeyboardControls/defaultcontrols");
        List<string> defaultControls = new List<string>();
        List<string> generatedControls = new List<string>();
        string line = fileReader.ReadLine();
        // Reads the file
        while (line != null)
        {
            defaultControls.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        fileReader = new StreamReader(path);
        line = fileReader.ReadLine();
        // Reads the file
        while (line != null)
        {
            generatedControls.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        if (generatedControls.Count != defaultControls.Count)
        {
            throw new IndexOutOfRangeException("defaultcontrols and generatedcontrols are not the same length.");
        }

        Dictionary<Keys, Keys> controlScheme = new Dictionary<Keys, Keys>();
        for (int k = 0; k < defaultControls.Count; k++)
        {
            Keys defaultkey = (Keys)Enum.Parse(typeof(Keys), defaultControls[k]);
            Keys newkey = (Keys)Enum.Parse(typeof(Keys), generatedControls[k]);
            controlScheme.Add(defaultkey, newkey);
        }

        return controlScheme;
    }
}


