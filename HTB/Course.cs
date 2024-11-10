using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HTB;

public class Course
{
    private static List<Course> _extent = new List<Course>();
    private string _courseName;
    private string _difficultyLevel;
    private IContentType _contentType;
    private IAccessType _accessType;

    public static List<Course> Extent => _extent;

    public string CourseName
    {
        get => _courseName;
        set => _courseName = value;
    }

    public string DifficultyLevel
    {
        get => _difficultyLevel;
        set => _difficultyLevel = value;
    }

    public IContentType ContentType
    {
        get => _contentType;
        set => _contentType = value;
    }

    public IAccessType AccessType
    {
        get => _accessType;
        set => _accessType = value;
    }

    public Course(string courseName, string difficultyLevel, IContentType contentType, IAccessType accessType)
    {
        _courseName = courseName;
        _difficultyLevel = difficultyLevel;
        _contentType = contentType;
        _accessType = accessType;
        _extent.Add(this);
    }

    public void RegisterCourse()
    {
        Console.WriteLine($"Course {_courseName} registered with difficulty level {_difficultyLevel}.");
        Console.WriteLine($"Content Type: {_contentType.GetTypeDescription()}");
        Console.WriteLine($"Access Type: {_accessType.GetAccessDescription()}");
    }

    public static void SaveExtent(string filename = "course_extent.json")
    {
        var json = JsonSerializer.Serialize(_extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "course_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            _extent = JsonSerializer.Deserialize<List<Course>>(json) ?? new List<Course>();
        }
    }
}

public interface IContentType
{
    string GetTypeDescription();
}

public interface IAccessType
{
    string GetAccessDescription();
}

public class OSINT : IContentType
{
    private string _techniqueFocus;

    public string TechniqueFocus
    {
        get => _techniqueFocus;
        set => _techniqueFocus = value;
    }

    public OSINT(string techniqueFocus)
    {
        _techniqueFocus = techniqueFocus;
    }

    public string GetTypeDescription()
    {
        return $"OSINT with focus on {_techniqueFocus}";
    }
}

public class PenetrationTesting : IContentType
{
    private string _testingEnvironment;

    public string TestingEnvironment
    {
        get => _testingEnvironment;
        set => _testingEnvironment = value;
    }

    public PenetrationTesting(string testingEnvironment)
    {
        _testingEnvironment = testingEnvironment;
    }

    public string GetTypeDescription()
    {
        return $"Penetration Testing in environment: {_testingEnvironment}";
    }
}

public class Free : IAccessType
{
    private int _accessDuration;

    public int AccessDuration
    {
        get => _accessDuration;
        set => _accessDuration = value;
    }

    public Free(int accessDuration)
    {
        _accessDuration = accessDuration;
    }

    public string GetAccessDescription()
    {
        return $"Free access for {_accessDuration} days";
    }
}

public class Paid : IAccessType
{
    private int _price;

    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public Paid(int price)
    {
        _price = price;
    }

    public string GetAccessDescription()
    {
        return $"Paid access with price ${_price}";
    }
}
