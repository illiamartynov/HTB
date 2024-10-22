using System.Text.Json;

namespace HTB;

using System;
using System.Collections.Generic;

public class Course
{
    public static List<Course> Extent = new List<Course>(); 

    public string CourseName { get; set; }
    public string Level { get; set; }
    public bool IsCompleted { get; set; }

    
    public Course(string courseName, string level, bool isCompleted)
    {
        CourseName = courseName;
        Level = level;
        IsCompleted = isCompleted;
        Extent.Add(this); 
    }

    
    public void Enroll()
    {
        Console.WriteLine($"Enrolled in course: {CourseName}");
    }

    
    public void CompleteCourse()
    {
        IsCompleted = true;
        Console.WriteLine($"Course {CourseName} completed.");
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

