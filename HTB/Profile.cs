namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Profile
{
    public static List<Profile> Extent = new List<Profile>(); 

    public int Points { get; set; }
    public string AcademyLevel { get; set; }

    
    public Profile(int points, string academyLevel)
    {
        Points = points;
        AcademyLevel = academyLevel;
        Extent.Add(this);  
    }

    
    public void UpdateProfile(int points, string academyLevel)
    {
        Points = points;
        AcademyLevel = academyLevel;
    }

    
    public void ViewProfile()
    {
        Console.WriteLine($"Points: {Points}, Academy Level: {AcademyLevel}");
    }

    
    public static void SaveExtent(string filename = "profile_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    
    public static void LoadExtent(string filename = "profile_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Profile>>(json);
        }
    }
}
