using System;
using System.Collections.Generic;
using System.IO.Ports;

public static class SerialHandler
{
    public static SerialPort _port {get; private set;} = new SerialPort();

    /*function for (re)connecting with the Arduino*/
    public static void ConnectSerial()
    {
        /*keep doing this until the port is open*/
        while (!_port.IsOpen)
        {
            /*get all available ports*/
            string[] openPorts = SerialPort.GetPortNames();

            /*standard communication settings for the Arduino*/
            _port.BaudRate = 9600;
            _port.ReadTimeout = 500;

            /*if we did not find any available devices, retry*/
            if (openPorts.Length <= 0)
            {
                Console.WriteLine("No Serial devices found, retrying...");
                Biodeg_cli.Program.encounteredError = true;
            }

            /*if we did:*/
            else
            {
                /*Go through them all*/
                foreach(string op in openPorts)
                {
                    /*Attempt to communicate with the device*/
                    try
                    {
                        /*Open the current port*/
                        _port.PortName = op;
                        _port.Open();
                        
                        /*Write a capital T to the device and read the response*/
                        _port.Write("T\n");
                        string res = _port.ReadLine();

                        /*If the response contains 'BioDeg v1', it is the arduino*/
                        if (res.Contains("BioDeg v1"))
                        {
                            /*Tell the user we found the arduino and exit the loop right away*/
                            Console.WriteLine($"Found Arduino on port {op}.");
                            Console.WriteLine("| Next log | CH 1 | CH 2 | CH 3 | CH 4 | CH 5 | CH 6 | CH 7 | CH 8 | CH 9 |");
                            break;
                        }

                        /*If we got no response or the wrong one, clean up after ourselves*/
                        else
                        {
                            _port.Close();
                        }
                    }

                    /*Device is either already in use by another program or an error occurred*/
                    catch
                    {
                        Console.WriteLine($"Device on port {op} is busy or failed to communicate");
                        Biodeg_cli.Program.encounteredError = true;
                    }
                }
            }
            Thread.Sleep(1000); /*retry after 1000 ms*/
        }
    }

    /*This function is for getting the data from the arduino*/
    public static int[] MeasureData()
    {
        /*Results go in here*/
        List<int> results = new List<int>();

        /*Keep trying until we have 9 results in the results list*/
        while (results.Count < 9)
        {
            /*Empty the current list to be sure nothing is in there -> prevents duplicate or incorrrect order of data in case it fails to read all values*/
            results = new List<int>();

            /*Only try and communicate if the port is open*/
            if (_port.IsOpen)
            {
                /*Write a capital M to the arduino, signifying we want data*/
                _port.Write("M\n");

                /*Try to read out all 9 values*/
                for (int i = 0; i < 9; i++)
                {
                    try
                    {
                        /*get next line out*/
                        string line = _port.ReadLine();
                        /*try to convert the string to an integer*/
                        int val = Int32.Parse(line);
                        /*scale the read value [0; 1023] to [0; 5000] to convert from value to mV*/
                        int mV = (int)(((double)val / 1023d) * 5000d);
                        results.Add(mV);
                    }

                    /*Tell the user that the conversion failed*/
                    catch
                    {
                        Console.WriteLine($"Failed to parse measurement, retrying...");
                        Biodeg_cli.Program.encounteredError = true;
                        Console.WriteLine(_port.IsOpen);
                        Console.WriteLine(_port.BytesToRead);
                        break;
                    }
                }
            }

            /*If the port is closed, try to reopen it first*/
            else
            {
                ConnectSerial();
            }

            Thread.Sleep(1000);
        }

        /*Convert to the proper data type*/
        return results.ToArray();
    }

    public static int[] MeasureDataNew()
    {
        return new int[] {};
    }
}