using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class Logger 
{
    static private Logger instance = null;

    private string fullPath;
    private System.IO.TextWriter writer;


    static public Logger GetInstance()
    {
        if (instance == null)
            instance = new Logger();

        return instance;
    }

    private Logger()
    {
        string path = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        fullPath = System.IO.Path.Combine(Application.persistentDataPath, path);
    }

    public void OpenFile()
    {
        if (writer != null)
            return;

        writer = System.IO.File.AppendText(fullPath + ".txt");
    }

    public void WriteLine(string str)
    {
        if (writer != null)
            writer.WriteLine(str);
    }

    public void CloseFile()
    {
        if (writer != null)
        {
            writer.Dispose();
            writer = null;
        }
    }
}
