namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Attempt
{
    public static List<Attempt> Extent = new List<Attempt>();  

    public int AttemptID { get; set; }
    public DateTime Timestamp { get; set; }
    public string Result { get; set; }

    
    public Attempt(int attemptID, DateTime timestamp, string result)
    {
        AttemptID = attemptID;
        Timestamp = timestamp;
        Result = result;
        Extent.Add(this);  
    }

    
    public static void SaveExtent(string filename = "attempt_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    
    public static void LoadExtent(string filename = "attempt_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Attempt>>(json);
        }
    }
}
