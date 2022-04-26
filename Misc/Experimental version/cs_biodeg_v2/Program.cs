using System;

public class Program
{
    public static void Main(string[] args)
    {
        SettingsHandler.LoadSettings(); //load in the settings from settings.json
        while (true)
        {
            SerialHandler.ConnectSerial();
        }
        Util.GenerateHeaders();
        SettingsHandler.filePath = InputHandler.GetFileName(); //add filepath to SettingsHandler for access across entire program
        Writer.CreateFile(SettingsHandler.filePath);
    }

    public static void RunProgram()
    {

    }
}