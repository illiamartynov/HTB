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
    }

    public void UpdateProfile(int points, string academyLevel)
    {
        Points = points;
        AcademyLevel = academyLevel;
    }

    public void ViewProfile()
    {
        Console.WriteLine($"Points: {_points}, Academy Level: {_academyLevel}");
    }

    // funcs for person connection
    public void AddPerson(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (Person != person) 
            throw new ArgumentException("The person already exists in the Profile class", nameof(person));

        Person = null;
        person.AddProfileReverse(this);
    }

    public void RemovePerson(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (Person != person)
            throw new ArgumentException("Isn't the right person", nameof(person));
    
        // remove Person from Profile
        Person = null;

        // remove Profile from person
        person.RemoveProfileReverse(this);
    }

    public void UpdatePerson(Person oldPerson, Person newPerson)
    {
        if (oldPerson == null)
            throw new ArgumentNullException(nameof(oldPerson));
        if (newPerson == null)
            throw new ArgumentNullException(nameof(newPerson));
        if (Person != oldPerson)
            throw new ArgumentException("The oldPerson doesn't exist in Profile class", nameof(oldPerson));
        if (Person == newPerson)  
            throw new ArgumentException("The person already exists in the Profile class", nameof(newPerson));
    
        Person = null;
        oldPerson.RemoveProfileReverse(this);

        // add new person
        Person = newPerson;
        newPerson.AddProfileReverse(this);
    }

    // Person reverse funcs
    public void AddProfileReverse(Person person)
    {
        Person = person;
    }

    public void RemoveProfileReverse(Person person)
    {
        Person = person;
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
