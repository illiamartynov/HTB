namespace HTB;

using System;
using System.Collections.Generic;

public class Admin : Person
{
    public int AdminID { get; set; }

    public Admin(string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive, int balance, int adminID, Profile profile, Leaderboard leaderboard)
        : base(email, name, password, registrationDate, birthDate, isActive, balance, profile, leaderboard)
    {
        AdminID = adminID;
    }

    public void CreateCourse(Course course)
    {
        Console.WriteLine($"Admin {Name} created course: {course.CourseName}");
    }

    public void ModifyUser(User user)
    {
        Console.WriteLine($"Admin {Name} modified user {user.Name}");
    }

    public void ViewReports()
    {
        Console.WriteLine($"Admin {Name} viewed reports.");
    }
}