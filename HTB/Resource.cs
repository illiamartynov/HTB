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

    public Lesson Lesson { get; private set; }

    public Resource(string resourceName, string resourceType, string url, Lesson lesson)
    {
        _resourceName = resourceName;
        _resourceType = resourceType;
        _url = url;

        AssignLesson(lesson);
        Resources.Add(this);
    }

    public void AssignLesson(Lesson lesson)
    {
        Lesson = lesson;
        lesson.AddResource(this);
    }

    public void UnassignLesson()
    {
        if (Lesson != null)
        {
            Lesson.RemoveResource(this);
            Lesson = null;
        }
    }

    public static void DeleteResource(Resource resource)
    {
        Resources.Remove(resource);
        resource.UnassignLesson();
    }
}