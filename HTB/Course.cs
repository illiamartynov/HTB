using System.Text.Json;

namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;

public class Course
{
    public static List<Course> Extent = new List<Course>(); 

    public string CourseName { get; set; }

    public Course(string courseName)
    {
        CourseName = courseName;
        Extent.Add(this); 
    }

    public void Enroll()
    {
        Console.WriteLine($"Enrolled in course: {CourseName}");
    }

    public void TrackProgress()
    {
        Console.WriteLine($"Tracking progress in course: {CourseName}");
    }

    public static void SaveExtent(string filename = "course_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "course_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Course>>(json);
        }
    }
}