using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

public class SettingsHelper
    {
    StreamReader fileReader;
    List<string> defaultControls = new List<string>();
    List<string> generatedControls = new List<string>();


    //genereert keyboard dictionary
    public Dictionary<Keys, Keys> GenerateKeyboardControls(string path)
    {
        path = "Content/" + path;
        ReadControlsFile("Content/Assets/Controls/defaultcontrols.txt", path);

        Dictionary<Keys, Keys> controlScheme = new Dictionary<Keys, Keys>();

            for (int k = 0; k < defaultControls.Count; k++)
            {
                Keys defaultkey = (Keys)Enum.Parse(typeof(Keys), defaultControls[k]);
                Keys newkey = (Keys)Enum.Parse(typeof(Keys), generatedControls[k]);
                controlScheme.Add(defaultkey, newkey);
            }

        generatedControls.Clear();
        defaultControls.Clear();

        return controlScheme;
    }


    //genereert xbox controller dictionary
    public Dictionary<Buttons, Buttons> GenerateXboxControls(string path)
    {
        path = "Content/" + path;        
        ReadControlsFile("Content/Assets/Controls/XboxControls/defaultXboxcontrols.txt", path);

        Dictionary<Buttons, Buttons> controlScheme = new Dictionary<Buttons, Buttons>();
        for (int k = 0; k < defaultControls.Count; k++)
        {
            Buttons defaultkey = (Buttons)Enum.Parse(typeof(Buttons), defaultControls[k]);
            Buttons newButtons = (Buttons)Enum.Parse(typeof(Buttons), generatedControls[k]);
            controlScheme.Add(defaultkey, newButtons);
        }
        generatedControls.Clear();
        defaultControls.Clear();
        return controlScheme;
    }



    //linkt de default controls aan de generatedcontrols in de dictionary
    public List<string> ReadControlsFile(string defaultFilePath, string generateFilePath)
    {
        fileReader = new StreamReader(defaultFilePath);
        string line = fileReader.ReadLine();
        // Reads the file
        while (line != null)
        {
            defaultControls.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        fileReader = new StreamReader(generateFilePath);
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
    return defaultControls;
    }
}


