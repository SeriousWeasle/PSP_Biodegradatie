using System;

public static class Writer
{
    public static string sysfile = "./sysfile";
    public static void CreateFile(string path)
    {
        /*Ensure that there is no file containing anything at the specified path -> prevents appending to an old dataset*/
        if(File.Exists(path)) { File.Delete(path); }
        if(File.Exists(sysfile)) { File.Delete(sysfile); }

        /*Make a new file with headers*/
        File.WriteAllText(sysfile, Util.fileHeader);
        File.Copy(sysfile, path);
    }
}