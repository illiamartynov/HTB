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

    public List<Certificate> Certificates { get; set; } = new List<Certificate>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public Leaderboard Leaderboard { get; set; }
    public List<Person> ReferredUsers { get; private set; } = new List<Person>();
    public Person() { }

    public Person(string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive, int balance, Profile profile, Leaderboard leaderboard)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is required");

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

    public void AddNotification(Notification notification)
    {
        Notifications.Add(notification);
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
    
    public void AddReferredUser(Person referredUser)
    {
        if (referredUser != null && !ReferredUsers.Contains(referredUser))
        {
            ReferredUsers.Add(referredUser);
            Console.WriteLine($"{Name} invited a new user: {referredUser.Name}");
        }
    }
}
