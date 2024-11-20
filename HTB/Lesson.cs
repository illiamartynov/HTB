using System.Text.Json;

namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Lesson
{
    private static List<Lesson> _extent = new List<Lesson>();
    private string _lessonTitle;
    private string _content;

    public static List<Lesson> Extent
    {
        get => _extent;
        private set => _extent = value;
    }

    [Required(ErrorMessage = "Lesson title is required.")]
    [StringLength(100, ErrorMessage = "Lesson title cannot exceed 100 characters.")]
    public string LessonTitle
    {
        get => _lessonTitle;
        set => _lessonTitle = value;
    }

    [Required(ErrorMessage = "Content is required.")]
    [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
    public string Content
    {
        get => _content;
        set => _content = value;
    }

    public Lesson(string lessonTitle, string content)
    {
        _lessonTitle = lessonTitle;
        _content = content;
        _extent.Add(this);
    }

    public void StartLesson()
    {
        Console.WriteLine($"Lesson {_lessonTitle} started.");
    }

    public void ViewResources()
    {
        Console.WriteLine($"Viewing resources for lesson: {_lessonTitle}");
    }

    public static void SaveExtent(string filename = "lesson_extent.json")
    {
        var json = JsonSerializer.Serialize(_extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "lesson_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            _extent = JsonSerializer.Deserialize<List<Lesson>>(json);
        }
    }
}