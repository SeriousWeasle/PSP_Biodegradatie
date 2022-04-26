using System;
using Newtonsoft.Json;


/**/
public static class SettingsHandler
{
    public static Settings settings { get; private set; } //settings will be stored in this variable
    public static int interval;
    public static string filePath;
    /*Like the name implies, this function loads the settings into the program*/
    public static void LoadSettings()
    {
        /*if the settings file exists, just load it*/
        if (File.Exists("./settings.json"))
        {
            string filestr = File.ReadAllText("./settings.json");
            settings = JsonConvert.DeserializeObject<Settings>(filestr);
            interval = settings.interval.GetAsSeconds();
        }

        /*if not, generate a new file with interval of 1 hour*/
        else
        {
            settings = new Settings(new TimeNotation(0, 0, 1));
            File.WriteAllText("./settings.json", JsonConvert.SerializeObject(settings));
        }
    }
}

/*For loading time from JSON file*/
[System.Serializable]
public struct TimeNotation
{
    public int seconds; //seconds of the interval
    public int minutes; //minutes of the interval
    public int hours; //hours of the interval

    public TimeNotation(int seconds, int minutes, int hours)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.hours = hours;
    }

    /*This function gets the TimeNotation as an integer in seconds -> converts to useful value for comparing*/
    public int GetAsSeconds()
    {
        return (this.hours * 60 * 60) + (this.minutes * 60) + this.seconds;
    }
}

/*for storing all data from JSON file*/
[System.Serializable]
public struct Settings
{
    public TimeNotation interval; //stores the set interval in a TimeNotation struct

    public Settings(TimeNotation interval)
    {
        this.interval = interval;
    }
}