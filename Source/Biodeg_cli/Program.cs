using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Biodeg_cli
{
    public class Program
    {
        /*used for telling the code if we need to print a new header*/
        public static bool encounteredError = false;
        /*Default measurement interval*/
        static float interval = 10f;
        /*amount of completed measurements*/
        static int measurements = 0;
        /*This holds the settings from settings.json*/
        static AppSettings settings;
        /*Entry point of the program*/
        public static void Main(string[] args)
        {
            /*First, load the settings from settings.json*/
            LoadSettings();

            /*Prompt the user for the filename*/
            Console.Write("Enter an output file name: ");
            string fileName = Console.ReadLine();

            /*if check if the file exists*/
            if (File.Exists($"./output/{fileName}.csv"))
            {
                /*if it does, ask the user if it should be overwritten*/
                Console.Write($"{fileName}.csv already exists. Do you want to overwrite it?\nAll data in the original file will be lost. (y/n)\n> ");
                string res = Console.ReadLine().ToLower();

                /*if the response is a yes, go ahead, else just exit*/
                if (res == "y" || res == "yes" || res == "ja" || res == "j")
                {
                    RunProgram(fileName);
                }
            }

            /*if it does not exist yet, run the program*/
            else
            {
                RunProgram(fileName);
            }
        }

        /*Main program loop*/
        public static void RunProgram(string fn)
        {
            /*Open a connection to the Arduino*/
            Console.WriteLine("Starting Serial connection");
            SerialHandler.ConnectSerial();
            Writer.CreateFile(fn);

            /*Store the start time and the time for the next measurement*/
            DateTime startTime = DateTime.Now;
            DateTime nextTime = startTime.AddSeconds(interval);

            /*Main program loop*/
            while (true)
            {
                /*Get the system time now*/
                DateTime currentTime = DateTime.Now;

                /*If the port is open, continue*/
                if (SerialHandler._port.IsOpen)
                {
                    /*If we ran into an error, reprint the data labels*/
                    if (encounteredError)
                    {
                        Console.WriteLine();
                        Console.WriteLine("| Next log | CH 1 | CH 2 | CH 3 | CH 4 | CH 5 | CH 6 | CH 7 | CH 8 | CH 9 |");
                        encounteredError = false;
                    }

                    /*Use a StringBuilder to build a list of backtrace characters*/
                    StringBuilder builder = new StringBuilder();
                    /*Generic format string for determining string length*/
                    string printstr = $"| {(nextTime - currentTime).ToString(@"hh\:mm\:ss")} | CH 1 | CH 2 | CH 3 | CH 4 | CH 5 | CH 6 | CH 7 | CH 8 | CH 9 |";
                    builder.Append('\b', printstr.Length);

                    /*Calculate based on system time if we should retry to measure*/
                    int currentMeasurements = (int)MathF.Ceiling((float)(currentTime - startTime).TotalMilliseconds / (interval * 1000f));
                    /*Check if we should measure now; and account in this check for daylight saving time by measuring if we passed the time or if the time to the next measurement is greater than the interval*/
                    if (currentMeasurements > measurements || currentTime > nextTime || (nextTime - currentTime).TotalMilliseconds > (interval * 1000f))
                    {
                        /*Tell the user we are currently writing to the file*/
                        Console.Write(builder + $"Logging values to {fn}.csv ...".PadRight(printstr.Length));

                        /*actually get data and write it to the file*/
                        int[] data = SerialHandler.MeasureData();
                        Writer.WriteToFile(data);

                        /*Update the time for the next measurements and update the measurements count; it is not incremented by 1 to account for system clock changes*/
                        nextTime = startTime.AddSeconds((float)currentMeasurements * interval);
                        measurements = currentMeasurements;
                    }
                    
                    /*if we are not going to log, write a nice live display*/
                    else
                    {
                        /*get data from Arduino*/
                        int[] data = SerialHandler.MeasureData();
                        if (data.Length >= 9)
                        {
                            /*Format it nicely*/
                            printstr = $"| {(nextTime - currentTime).ToString(@"hh\:mm\:ss")} | {pval(data[0])} | {pval(data[1])} | {pval(data[2])} | {pval(data[3])} | {pval(data[4])} | {pval(data[5])} | {pval(data[6])} | {pval(data[7])} | {pval(data[8])} |";
                            /*Go to the beginning of the line and print the live update*/
                            Console.Write(builder + printstr);
                        }
                    }
                }

                /*Reopen the serial connection*/
                else
                {
                    SerialHandler.ConnectSerial();
                }

                /*Wait 50 ms and do it all again*/
                Thread.Sleep(50);
            }
        }

        /*Pads values with spaces so they are 4 characters wide for formatting*/
        public static string pval(int num)
        {
            return num.ToString().PadLeft(4);
        }

        /*This function loads the settings from settings.json*/
        public static void LoadSettings()
        {
            string settingsString = File.ReadAllText("./settings.json"); //Get all text from the file
            settings = JsonConvert.DeserializeObject<AppSettings>(settingsString); //convert it into an AppSettings struct and store it in settings
            Console.WriteLine($"Got interval of {settings.interval.GetSeconds()} seconds from settings"); //Tell the user the new interval it found
            interval = settings.interval.GetSeconds(); //Apply the new interval
        }
    }
}

[System.Serializable]
public struct TimeNotation //time notation struct for settings
{
    /*Store time as hours, minutes, seconds*/
    public float seconds;
    public float minutes;
    public float hours;
    /*Constructor*/
    public TimeNotation(float seconds, float minutes, float hours)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.hours = hours;
    }
    /*Get this time in seconds*/
    public float GetSeconds()
    {
        return (this.hours * 60f * 60f) + (this.minutes * 60f) + this.seconds;
    }
}

[System.Serializable]
public struct AppSettings //struct for app settings
{
    public TimeNotation interval; //time notation for the interval

    /*constructor*/
    public AppSettings(TimeNotation interval)
    {
        this.interval = interval;
    }
}