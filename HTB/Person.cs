using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HTB;

public class Person
{
    public static List<Person> Extent = new List<Person>();

    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public int Balance { get; set; }
    public int Age { get; private set; }
    public Profile UserProfile { get; set; }
    public Address Address { get; set; } 
    public Rank Rank { get; set; } 
    public CompletenessLevel CompletenessLevel { get; set; } 

    public List<Certificate> Certificates { get; set; } = new List<Certificate>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public Leaderboard Leaderboard { get; set; }
    public List<Person> ReferredUsers { get; private set; } = new List<Person>();

    public Subscription Subscription { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
    public List<Challenge> Challenges { get; set; } = new List<Challenge>();

    public Person() { }

    public Person(
        string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive, 
        int balance, Profile profile, Leaderboard leaderboard, Address address, Rank rank, CompletenessLevel completenessLevel, 
        Subscription subscription)
    {
        Email = email;
        Name = name;
        Password = password;
        RegistrationDate = registrationDate;
        BirthDate = birthDate;
        IsActive = isActive;
        Balance = balance;
        Age = CalculateAge(birthDate);
        UserProfile = profile;
        Leaderboard = leaderboard;
        Address = address;
        Rank = rank;
        CompletenessLevel = completenessLevel;
        Subscription = subscription;
        Extent.Add(this);
    }

    public void Register()
    {
        Console.WriteLine($"{Name} registered.");
    }

    public void Login()
    {
        Console.WriteLine($"{Name} logged in.");
    }

    public void Logout()
    {
        Console.WriteLine($"{Name} logged out.");
    }

    public void AddCertificate(Certificate certificate)
    {
        Certificates.Add(certificate);
    }

    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
    }

    public void AddReferredUser(Person referredUser)
    {
        if (referredUser != null && !ReferredUsers.Contains(referredUser))
        {
            ReferredUsers.Add(referredUser);
            Console.WriteLine($"{Name} invited a new user: {referredUser.Name}");
        }
    }

    public void AddCourse(Course course)
    {
        if (course != null && !Courses.Contains(course))
        {
            Courses.Add(course);
            Console.WriteLine($"{Name} enrolled in course: {course.CourseName}");
        }
    }

    public void AddChallenge(Challenge challenge)
    {
        if (challenge != null && !Challenges.Contains(challenge))
        {
            Challenges.Add(challenge);
            Console.WriteLine($"{Name} accepted challenge: {challenge.ChallengeName}");
        }
    }

    public static void SaveExtent(string filename = "person_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "person_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Person>>(json);
        }
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}



public class CompletenessLevel
{
    public int CompletenessPercentage { get; set; }
    public DateTime StartDate { get; set; }

    public CompletenessLevel(int completenessPercentage, DateTime startDate)
    {
        CompletenessPercentage = completenessPercentage;
        StartDate = startDate;
    }

    public void UpdateCompleteness(int newPercentage)
    {
        if (newPercentage >= 0 && newPercentage <= 100)
        {
            CompletenessPercentage = newPercentage;
            Console.WriteLine($"Completeness updated to {newPercentage}%.");
        }
        else
        {
            Console.WriteLine("Invalid percentage value. Must be between 0 and 100.");
        }
    }
}


public class Rank
{
    public int RankLevel { get; set; }

    public Rank(int rankLevel)
    {
        RankLevel = rankLevel;
    }

    public void UpdateRank(int newRankLevel)
    {
        if (newRankLevel > 0)
        {
            RankLevel = newRankLevel;
            Console.WriteLine($"Rank updated to level {newRankLevel}.");
        }
        else
        {
            Console.WriteLine("Invalid rank level. Must be greater than 0.");
        }
    }
}
