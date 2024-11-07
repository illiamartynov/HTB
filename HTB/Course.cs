using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HTB;

public class Course
{
    public static List<Course> Extent = new List<Course>();

    public string CourseName { get; set; }
    public string DifficultyLevel { get; set; }
    public IContentType ContentType { get; set; }
    public IAccessType AccessType { get; set; }

    public Course(string courseName, string difficultyLevel, IContentType contentType, IAccessType accessType)
    {
        CourseName = courseName;
        DifficultyLevel = difficultyLevel;
        ContentType = contentType;
        AccessType = accessType;
        Extent.Add(this);
    }

    public void RegisterCourse()
    {
        Console.WriteLine($"Course {CourseName} registered with difficulty level {DifficultyLevel}.");
        Console.WriteLine($"Content Type: {ContentType.GetTypeDescription()}");
        Console.WriteLine($"Access Type: {AccessType.GetAccessDescription()}");
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
    public string TechniqueFocus { get; set; }

    public OSINT(string techniqueFocus)
    {
        TechniqueFocus = techniqueFocus;
    }

    public string GetTypeDescription()
    {
        return $"OSINT with focus on {TechniqueFocus}";
    }
}

public class PenetrationTesting : IContentType
{
    public string TestingEnvironment { get; set; }

    public PenetrationTesting(string testingEnvironment)
    {
        TestingEnvironment = testingEnvironment;
    }

    public string GetTypeDescription()
    {
        return $"Penetration Testing in environment: {TestingEnvironment}";
    }
}

public class Free : IAccessType
{
    public int AccessDuration { get; set; }

    public Free(int accessDuration)
    {
        AccessDuration = accessDuration;
    }

    public string GetAccessDescription()
    {
        return $"Free access for {AccessDuration} days";
    }
}

public class Paid : IAccessType
{
    public int Price { get; set; }

    public Paid(int price)
    {
        Price = price;
    }

    public string GetAccessDescription()
    {
        return $"Paid access with price ${Price}";
    }
}
