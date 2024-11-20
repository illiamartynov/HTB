namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

public class Profile
{
    public static List<Profile> Extent = new List<Profile>();

    private int _points;
    private string _academyLevel;

    [Required(ErrorMessage = "Points are required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Points cannot be negative.")]
    public int Points
    {
        get => _points;
        set => _points = value;
    }

    [Required(ErrorMessage = "Academy Level is required.")]
    [StringLength(50, ErrorMessage = "Academy Level cannot exceed 50 characters.")]
    public string AcademyLevel
    {
        get => _academyLevel;
        set => _academyLevel = value;
    }

    public Profile(int points, string academyLevel)
    {
        _points = points;
        _academyLevel = academyLevel;
        Extent.Add(this);
    }

    public void UpdateProfile(int points, string academyLevel)
    {
        _points = points;
        _academyLevel = academyLevel;
    }

    public void ViewProfile()
    {
        Console.WriteLine($"Points: {_points}, Academy Level: {_academyLevel}");
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