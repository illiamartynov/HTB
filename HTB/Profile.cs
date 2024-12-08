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

    public Person Person { get; private set; } // Связь с `Person`

    public Profile(int points, string academyLevel, Person person)
    {
        Points = points >= 0 ? points : throw new ArgumentOutOfRangeException(nameof(points));
        AcademyLevel = academyLevel ?? throw new ArgumentNullException(nameof(academyLevel));
        Person = person ?? throw new ArgumentNullException(nameof(person));
    }

    public void UpdateProfile(int points, string academyLevel)
    {
        Points = points >= 0 ? points : throw new ArgumentOutOfRangeException(nameof(points));
        AcademyLevel = academyLevel ?? throw new ArgumentNullException(nameof(academyLevel));
    }

    public void ViewProfile()
    {
        Console.WriteLine($"Points: {_points}, Academy Level: {_academyLevel}");
    }

    // Присвоение человека к профилю
    public void AssignPerson(Person person)
    {
        if (Person != null && Person != person)
            throw new InvalidOperationException("This profile is already assigned to another person.");

        Person = person;
    }


    // Удаление связи с человеком
    public void UnassignPerson()
    {
        Person = null;
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
