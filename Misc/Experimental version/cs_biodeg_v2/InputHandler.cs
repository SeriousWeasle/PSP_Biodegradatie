using System;

public static class InputHandler
{
    public static string GetFileName()
    {
        /*Initialize variables*/
        bool hasFileName = false;
        string fileName = "default";
        string filePath = $"./output/{fileName}.csv";

        /*make sure the output directory exists to prevent errors*/
        if (!Directory.Exists("./output")) {Directory.CreateDirectory("./output"); }

        /*keep prompting until either a new name is chosen or a file can be overwritten*/
        while (!hasFileName)
        {
            /*Ask for a filename*/
            Console.Write("Please enter a filename:\n> ");
            fileName = Console.ReadLine();

            /*Check if the file exists*/
            filePath = $"./output/{fileName}.csv";
            if (File.Exists(filePath))
            {
                /*Ask the user if the file can be overwritten*/
                Console.Write($"The file '{fileName}.csv' already exists, do you want to overwrite it? WARNING - all data in the original file will be LOST. (y/n)\n> ");
                string res = Console.ReadLine().ToLower();
                if (res == "y" || res == "yes" || res == "ja" || res == "y")
                {
                    hasFileName = true;
                }
            }

            else
            {
                hasFileName = true;
            }
        }

        /*Return the chosen filepath to where the function was called*/
        return filePath;
    }
}