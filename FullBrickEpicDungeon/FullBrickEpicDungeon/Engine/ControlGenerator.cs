using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

// Class that generates controls from files
public class ControlGenerator
    {
    StreamReader fileReader;
    List<string> defaultControls = new List<string>();
    List<string> generatedControls = new List<string>();

    //Generates the keyboard dictionary -> defaultcontrols, actualcontrols
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

    //reads the control files
    public List<string> ReadControlsFile(string defaultFilePath, string generateFilePath)
    {
        fileReader = new StreamReader(defaultFilePath);
        string line = fileReader.ReadLine();
        // Reads the defaultcontrols file
        while (line != null)
        {
            defaultControls.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        fileReader = new StreamReader(generateFilePath);
        line = fileReader.ReadLine();
        // Reads the generatedcontrols file
        while (line != null)
        {
            generatedControls.Add(line);
            line = fileReader.ReadLine();
        }
        fileReader.Close();
        // return an error if generatedcontrols and defaultcontrols are not the same length, this is because this might cause errors if left undetected
        if (generatedControls.Count != defaultControls.Count)
        {
            throw new IndexOutOfRangeException("defaultcontrols and generatedcontrols are not the same length.");
        }
    return defaultControls;
    }
}


