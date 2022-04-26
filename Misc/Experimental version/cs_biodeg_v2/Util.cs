using System;

public static class Util
{
    public static string fileHeader = "placeholder\n";
    public static string readoutHeader = "placeholder";
    public static int numChambers = 9;

    public static void GenerateHeaders()
    {
        fileHeader = "Timestamp, ";
        readoutHeader = "| Next log |";

        for (int n = 0; n < numChambers; n++)
        {
            if (n != numChambers - 1) { fileHeader += $"CH{n + 1}, "; }
            else { fileHeader += $"CH{n + 1}"; }

            readoutHeader +=  $"CH{(n + 1).ToString().PadLeft(2)} |";
        }
    }
}