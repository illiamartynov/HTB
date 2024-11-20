using System.Text.Json.Serialization;

namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

public class Person
{
    private static List<Person> _extent = new List<Person>();
    public static IReadOnlyList<Person> Extent => _extent;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [JsonIgnore]
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    private string _password;

    public string Password
    {
        get => "****";
        set => _password = HashPassword(value);
    }

    [Required(ErrorMessage = "Registration date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Registration date must be a valid date.")]
    public DateTime RegistrationDate { get; set; }

    [Required(ErrorMessage = "Birth date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Birth date must be a valid date.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Balance cannot be negative.")]
    public int Balance { get; set; }

    [Required(ErrorMessage = "User profile is required.")]
    public Profile UserProfile { get; set; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
    public int Age { get; private set; }

    public Address? Address { get; set; } 

    [Required(ErrorMessage = "Rank is required.")]
    public Rank Rank { get; set; }

    [Required(ErrorMessage = "Completeness level is required.")]
    public CompletenessLevel CompletenessLevel { get; set; }

    public IReadOnlyList<Certificate> Certificates => _certificates;

    private List<Certificate> _certificates = new List<Certificate>();

    public IReadOnlyList<Payment> Payments => _payments;

    private List<Payment> _payments = new List<Payment>();

    [Required(ErrorMessage = "Leaderboard is required.")]
    public Leaderboard Leaderboard { get; set; }

    public IReadOnlyList<Person> ReferredUsers => _referredUsers;

    private List<Person> _referredUsers = new List<Person>();

    [Required(ErrorMessage = "Subscription is required.")]
    public Subscription Subscription { get; set; }

    public IReadOnlyList<Course> Courses => _courses;

    private List<Course> _courses = new List<Course>();

    public IReadOnlyList<Challenge> Challenges => _challenges;

    private List<Challenge> _challenges = new List<Challenge>();

    public Person() {}

    public Person(
        string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive,
        int balance, Profile profile, Leaderboard leaderboard, Address? address, Rank rank, CompletenessLevel completenessLevel,
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
