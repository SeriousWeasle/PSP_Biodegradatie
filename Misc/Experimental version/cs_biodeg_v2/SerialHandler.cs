using System;
using System.Collections.Generic;
using System.IO.Ports;

public static class SerialHandler
{
    public static SerialPort _port = new SerialPort();

    public static void ConnectSerial()
    {
        /*Standard port settings*/
        _port.BaudRate = 9600;
        _port.WriteTimeout = 500;
        _port.ReadTimeout = 500;

        bool hasFoundArduino = false;

        while (!hasFoundArduino)
        {
            /*Get a list of available devices*/
            string[] availableDevices = SerialPort.GetPortNames();

            /*go over all available serial devices*/
            foreach(string device in availableDevices)
            {
                /*Ensure that the port is closed before changing variables*/
                if (_port.IsOpen) { _port.Close(); }

                /*Select the device*/
                _port.PortName = device;

                /*Attempt to communicate*/
                try
                {
                    /*open the port*/
                    _port.Open();

                    /*Write capital T*/
                    _port.Write("T\n");

                    /*wait 1 second so the Arduino has time to respond*/
                    Thread.Sleep(1000);

                    /*read out the data and check if it is the BioDeg arduino*/
                    string res = ReadPort();

                    /*If the response contains text and the validator can verify that the Arduino is running the correct software, break from the loop*/
                    if (res.Length > 0)
                    {
                        if (VerifyConnectionString(res))
                        {
                            /*Announce that we have found the Arduino*/
                            Console.WriteLine($"Found Arduino on port {device}.");
                            hasFoundArduino = true;
                            Console.WriteLine(Util.numChambers);
                            break;
                        }
                    }
                }

                /*Communication failed on this port*/
                catch
                {
                    Console.WriteLine($"Could not communicate with device on port {device}.");
                }
            }

            /*Only tell user the Arduino was not found if it was actually not found*/
            if (!hasFoundArduino)
            {
                Console.WriteLine("Could not find Arduino, retrying...");
                Thread.Sleep(1000);
            }
        }
    }

    public static int[] Measure()
    {
        List<int> results = new List<int>();
        bool measurementsValid = false;

        while (!measurementsValid)
        {
            if (_port.IsOpen)
            {

            }

            else 
            {
                Console.WriteLine("Lost connection, retrying...");
                ConnectSerial();
            }

            if (!measurementsValid) { Thread.Sleep(1000); }
        }

        return results.ToArray();
    }

    static bool VerifyConnectionString(string str)
    {
        /*split out all substrings*/
        string[] cmds = str.Split(':');

        /*bools for storing verification step results*/
        bool versionNumCorrect = false;
        bool chamberNumCorrect = false;

        /*check version number*/
        if (str.Contains("BioDeg v2"))
        {
            versionNumCorrect = true;
        }

        /*read through all commands*/
        bool isReadingCols = false; //stores if INIT_START is encountered
        List<int> cols = new List<int>();
        foreach (string cmd in cmds)
        {
            //If INIT_START was read, the column numbers will be written next
            if (cmd == "INIT_START")
            {
                isReadingCols = true;
            }

            //Signifies it is the end of reading column numbers
            else if (cmd == "INIT_END")
            {
                isReadingCols = false;
            }

            //if we are reading column numbers, try to convert the cmd to an int and add it to the cols array
            else if (isReadingCols)
            {
                try
                {
                    cols.Add(Int32.Parse(cmd));
                }

                catch {}
            }
        }

        //check if we received 2 column numbers and if they are both the same
        if (cols.Count == 2)
        {
            if (cols[0] == cols[1])
            {
                chamberNumCorrect = true;
                Util.numChambers = cols[0];
            }
        }

        //return true if both version number and column numbers are correct
        return versionNumCorrect && chamberNumCorrect;
    }

    static string ReadPort()
    {
        /*local variables for reading out n bytes from the port*/
        int numToRead = _port.BytesToRead;
        byte[] rdBytes = new byte[numToRead];

        /*read out n bytes*/
        _port.Read(rdBytes, 0, numToRead);

        /*convert bytes to ascii chars*/
        List<char> chars = new List<char>();
        foreach (byte b in rdBytes)
        {
            char c = (char)b;
            if (c != '\n')
            {
                chars.Add(c);
            }
        }

        Console.WriteLine(new string(chars.ToArray()));

        /*return the array of ascii chars as a string*/
        return new string(chars.ToArray());
    }
}