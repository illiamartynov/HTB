namespace HTB;

using System;
using System.Collections.Generic;

public class User : Person
{
    public string Username { get; set; }
    public List<Course> EnrolledCourses { get; set; } = new List<Course>();

    public User(string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive, int balance, string username, Profile profile, Leaderboard leaderboard)
        : base(email, name, password, registrationDate, birthDate, isActive, balance, profile, leaderboard)
    {
        Username = username;
    }

    public void AddCourse(Course course)
    {
        EnrolledCourses.Add(course);
        Console.WriteLine($"Added course: {course.CourseName} for user: {Username}");
    }

    public void ViewCourses()
    {
        Console.WriteLine($"Courses for user {Username}:");
        foreach (var course in EnrolledCourses)
        {
            Console.WriteLine(course.CourseName);
        }
    }
}