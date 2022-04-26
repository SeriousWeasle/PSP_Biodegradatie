using System;
public class Writer
{
    /*Standard filename in case something goes wrong*/
    static string _path = "./default.csv";
    /*internal backup file in case excel or another software is opening the .csv*/
    static string sysfile = "./sysfile";
    /*This function creates the output folder and makes a new .csv file*/
    public static void CreateFile(string name)
    {
        /*check if output folder exists*/
        if (!Directory.Exists("./output"))
        {
            Directory.CreateDirectory("./output");
        }
        /*set path to specified name*/
        _path = $"./output/{name}.csv";
        /*remove the previous file if it exists*/
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
        /*write fileheaders to the systemfile and copy the data over into the CSV*/
        File.WriteAllText(sysfile, "Timestamp, CH1, CH2, CH3, CH4, CH5, CH6, CH7, CH8, CH9\n");
        File.Copy(sysfile, _path);
    }

    /*Function for writing an array of data to the CSV file*/
    public static void WriteToFile(int[] data)
    {
        /*Get a timestamp*/
        string writestr = $"{DateTime.Now}, ";

        /*Go over all integers in the data array*/
        for (int i = 0; i < data.Length; i++)
        {
            /*if it is the last one, add a line break*/
            if (i == data.Length - 1)
            {
                writestr += $"{data[i]}\n";
            }
            /*if not, add the number, a comma and a space*/
            else
            {
                writestr += $"{data[i]}, ";
            }
        }

        /*Write the new line to the systemfile*/
        File.AppendAllText(sysfile, writestr);

        /*Attempt to copy the data to the CSV*/
        try
        {
            File.Delete(_path);
            File.Copy(sysfile, _path);
        }

        /*If it can't, the CSV file is in use by another program*/
        catch
        {
            Console.WriteLine("Could not write output to csv, make sure other progams are closed");
            Biodeg_cli.Program.encounteredError = true;
        }
    }
}