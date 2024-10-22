using System.Text.Json;

namespace HTB;

using System;
using System.Collections.Generic;

public class Lesson
{
    public static List<Lesson> Extent = new List<Lesson>(); 

    public string LessonTitle { get; set; }
    public string Content { get; set; }

    
    public Lesson(string lessonTitle, string content)
    {
        LessonTitle = lessonTitle;
        Content = content;
        Extent.Add(this); 
    }

    
    public void StartLesson()
    {
        Console.WriteLine($"Lesson {LessonTitle} started.");
    }

    
    public void ViewResources()
    {
        Console.WriteLine($"Viewing resources for lesson: {LessonTitle}");
    }

    
    public static void SaveExtent(string filename = "lesson_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    
    public static void LoadExtent(string filename = "lesson_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Lesson>>(json);
        }
    }
}
