using System.Text.Json.Serialization;

namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Person
{
    private static List<Person> _extent = new List<Person>();
    public static IReadOnlyList<Person> Extent => _extent;

    public string Email { get; set; }
    public string Name { get; set; }
    private string _password;
    
    [JsonIgnore] 
    public string Password
    {
        get => "****";
        set => _password = HashPassword(value);
    }

    public DateTime RegistrationDate { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public int Balance { get; set; }
    public int Age { get; private set; }
    public Profile UserProfile { get; set; }
    public Address Address { get; set; }
    public Rank Rank { get; set; }
    public CompletenessLevel CompletenessLevel { get; set; }

    private List<Certificate> _certificates = new List<Certificate>();
    public IReadOnlyList<Certificate> Certificates => _certificates;

    private List<Payment> _payments = new List<Payment>();
    public IReadOnlyList<Payment> Payments => _payments;

    public Leaderboard Leaderboard { get; set; }
    private List<Person> _referredUsers = new List<Person>();
    public IReadOnlyList<Person> ReferredUsers => _referredUsers;

    public Subscription Subscription { get; set; }
    private List<Course> _courses = new List<Course>();
    public IReadOnlyList<Course> Courses => _courses;

    private List<Challenge> _challenges = new List<Challenge>();
    public IReadOnlyList<Challenge> Challenges => _challenges;

    public Person() {}

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
        _extent.Add(this);
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
        if (certificate != null && !_certificates.Contains(certificate))
        {
            _certificates.Add(certificate);
        }
    }

    public void AddPayment(Payment payment)
    {
        if (payment != null && !_payments.Contains(payment))
        {
            _payments.Add(payment);
        }
    }

    public void AddReferredUser(Person referredUser)
    {
        if (referredUser != null && !_referredUsers.Contains(referredUser))
        {
            _referredUsers.Add(referredUser);
            Console.WriteLine($"{Name} invited a new user: {referredUser.Name}");
        }
    }

    public void AddCourse(Course course)
    {
        if (course != null && !_courses.Contains(course))
        {
            _courses.Add(course);
            Console.WriteLine($"{Name} enrolled in course: {course.CourseName}");
        }
    }

    public void AddChallenge(Challenge challenge)
    {
        if (challenge != null && !_challenges.Contains(challenge))
        {
            _challenges.Add(challenge);
            Console.WriteLine($"{Name} accepted challenge: {challenge.ChallengeName}");
        }
    }

    public static void SaveExtent(string filename = "person_extent.json")
    {
        var json = JsonSerializer.Serialize(_extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "person_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            _extent = JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
        }
    }

    public static void ClearExtent()
    {
        _extent.Clear();
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
