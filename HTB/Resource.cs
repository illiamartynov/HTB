namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Resource
{
    public static List<Resource> Resources = new List<Resource>();

    private string _resourceName;
    private string _resourceType;
    private string _url;

    [Required(ErrorMessage = "Resource name is required.")]
    [StringLength(100, ErrorMessage = "Resource name cannot exceed 100 characters.")]
    public string ResourceName
    {
        get => _resourceName;
        set => _resourceName = value;
    }

    [Required(ErrorMessage = "Resource type is required.")]
    [StringLength(50, ErrorMessage = "Resource type cannot exceed 50 characters.")]
    public string ResourceType
    {
        get => _resourceType;
        set => _resourceType = value;
    }

    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public string Url
    {
        get => _url;
        set => _url = value;
    }

    public List<Lesson> Lessons { get; private set; }

    public Resource(string resourceName, string resourceType, string url, Lesson lesson)
    {
        _resourceName = resourceName;
        _resourceType = resourceType;
        _url = url;
    }
    
    // funcs for lesson connection
    public void AddLesson(Lesson lesson)
    {
        if (lesson == null)
            throw new ArgumentNullException(nameof(lesson));
        if (Lessons.Contains(lesson)) 
            throw new ArgumentException("The lesson already exists in the Resource class", nameof(lesson));

        Lessons.Add(lesson);
        lesson.AddResourceReverse(this);
    }

    public void RemoveLesson(Lesson lesson)
    {
        if (lesson == null)
            throw new ArgumentNullException(nameof(lesson));
        if (!Lessons.Contains(lesson))
            return;
        
        // remove Resource from Lesson
        Lessons.Remove(lesson);

        // remove Resource from lesson
        lesson.RemoveResourceReverse(this);
    }

    public void UpdateLesson(Lesson oldLesson, Lesson newLesson)
    {
        if (oldLesson == null)
            throw new ArgumentNullException(nameof(oldLesson));
        if (newLesson == null)
            throw new ArgumentNullException(nameof(newLesson));
        if (!Lessons.Contains(oldLesson))
            throw new ArgumentException("The oldLesson doesn't exist in Resource class", nameof(oldLesson));
        if (Lessons.Contains(newLesson))   
            throw new ArgumentException("The lesson already exists in the Resource class", nameof(newLesson));
        
        Lessons.Remove(oldLesson);
        oldLesson.RemoveResourceReverse(this);

        // add new lesson
        Lessons.Add(newLesson);
        newLesson.AddResourceReverse(this);
    }
    
    // Lesson reverse funcs
    public void AddLessonReverse(Lesson lesson)
    {
        Lessons.Add(lesson);
    }

    public void RemoveLessonReverse(Lesson lesson)
    {
        Lessons.Remove(lesson);
    }
}
